using SMSSender.Entities.Common;
using SMSSender.Entities.Contracts.DTOs.Worker;
using SMSSender.Entities.Models.DeviceConfig;
using SMSSender.Interfaces;
using SMSSender.Interfaces.Hub;
using SMSSender.Interfaces.Repositories;
using SMSSender.Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSSender.Services
{
    public class WorkerService : IWorkerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotificationService _notificationService;
        private readonly IHubNotificationService _hubNotificationService;
        public WorkerService(IUnitOfWork unitOfWork, INotificationService notificationService, IHubNotificationService hubNotificationService)
        {
            _unitOfWork = unitOfWork;
            _notificationService = notificationService;
            _hubNotificationService = hubNotificationService;
        }

        public async Task<DeviceWorkrtDto> GetAllDevices(Guid CustomerId, int BranchId, int Version)
        {
            if (Version == 0)
            {
                var Devices = await _unitOfWork.Repository<Device>().WhereAsync(x => x.CustomerId == CustomerId && x.BranchId == BranchId);
                return new DeviceWorkrtDto
                {
                    LatestVersion = 1,
                    Devices = Devices,
                    Changes = new List<DeviceChangeLog>()
                };
            }
            else
            {
                var Changes = await _unitOfWork.Repository<DeviceChangeLog>().WhereAsync(x => x.CustomerId == CustomerId && x.BranchId == BranchId && x.Version > Version);
                if (!Changes.Any())
                {
                    return new DeviceWorkrtDto
                    {
                        LatestVersion = 0,
                        Devices = new List<Device>(),
                        Changes = new List<DeviceChangeLog>()
                    };
                }
                var WorkerDto = new DeviceWorkrtDto
                {
                    LatestVersion = 0,
                    Devices = new List<Device>(),
                    Changes = new List<DeviceChangeLog>()
                };
                var LatestVersion = await _unitOfWork.Repository<DeviceSyncVersion>().FirstOrDefaultAsync(x => x.CustomerId == CustomerId && x.BranchId == BranchId);
                var DeviceIds = Changes.Select(i => i.DeviceId).Distinct().ToList();
                var Devices = await _unitOfWork.Repository<Device>().WhereAsync(x => x.CustomerId == CustomerId && DeviceIds.Contains(x.DeviceUniqueId));
                var AddedDeviceIds = Changes.Where(i => i.Action == Entities.Common.ChangeLogAction.Created).Select(i => i.DeviceId).Distinct().ToList();
                if (AddedDeviceIds.Any())
                {
                    var AddedDevices = Devices.Where(i => AddedDeviceIds.Contains(i.DeviceUniqueId)).ToList();
                    WorkerDto.Devices = AddedDevices;
                }

                WorkerDto.LatestVersion = LatestVersion.CurrentVersion;
                WorkerDto.Changes = Changes.Where(i => i.Action != Entities.Common.ChangeLogAction.Created).ToList();
                return WorkerDto;
            }
        }

        public async Task<RefreshInboxDto> GetDeviceRefreshInboxSetting(Guid CustomerId, int BranchId)
        {
            var Setting = await _unitOfWork.Repository<DeviceRefreshInbox>().FirstOrDefaultAsync(x => x.CustomerId == CustomerId && x.BranchId == BranchId && x.RefreshStatus == Entities.Common.InboxRefreshStatus.Pending);
            if (Setting == null)
                return null;

            return new RefreshInboxDto
            {
                DeviceId = Setting.DeviceId,
                From = Setting.From,
                To = Setting.To
            };
        }

        public async Task<bool> UpdateDeviceRefreshInboxStatus(Guid CustomerId, int BranchId, string DeviceId)
        {
            var Setting = await _unitOfWork.Repository<DeviceRefreshInbox>().FirstOrDefaultAsync(x => x.CustomerId == CustomerId && x.BranchId == BranchId && x.DeviceId == DeviceId);
            var Device = await _unitOfWork.Repository<Device>().FirstOrDefaultAsync(x => x.CustomerId == CustomerId && x.BranchId == BranchId && x.DeviceUniqueId == DeviceId);
            if (Setting != null)
            {
                Setting.RefreshStatus = Entities.Common.InboxRefreshStatus.Completed;
                string NotBody = $"تم تحديث صندوق الرسائل لجهاز ({Device.DeviceName}). الفترة من {Setting.From?.ToString("")} الى {Setting.To?.ToString("")}";
                _notificationService.CreateSystemNotification("تحددث صندوق الرسائل", NotBody, CustomerId, BranchId);
                await _unitOfWork.CompleteAsync();
                await _hubNotificationService.SendSystemMessageAddedAsync(CustomerId, BranchId);

                return true;
            }
            else
                return false;
        }

        public async Task<ApiResponseModel<string>> UpdateDeviceHealth(List<DeviceHealthDetailsRequest> Model)
        {
            try
            {
                var CustomerId = Model.FirstOrDefault()?.CustomerId;
                var BranchId = Model.FirstOrDefault()?.BranchId;
                var Today = DateTime.Now.EgyptNow();
                var Entities = await _unitOfWork.Repository<DeviceHealth>().WhereAsync(i => i.CustomerId == CustomerId && i.BranchId == BranchId);
                if (Entities.Count == 0)
                {
                    var Details = Model.Select(i => new DeviceHealth
                    {
                        DeviceId = i.DeviceId,
                        CustomerId = CustomerId.Value,
                        BranchId = BranchId.Value,
                        DeviceName = i.DeviceName,
                        MessagesFailed = i.MessagesFailed,
                        ConnectionStatus = i.ConnectionStatus,
                        ConnectionTransport = i.ConnectionTransport,
                        BatteryLevel = i.BatteryLevel,
                        BatteryCharging = i.BatteryCharging,
                        LastSeen = i.LastSeen,
                        LastSyncDate = i.LastSyncDate,
                        LastUpdated = Today
                    }).ToList();

                    await _unitOfWork.Repository<DeviceHealth>().AddRangeAsync(Details);
                }
                else
                {
                    var modelLookup = Model.ToDictionary(x => x.DeviceId);

                    foreach (var entity in Entities)
                    {
                        if (modelLookup.TryGetValue(entity.DeviceId, out var modelEntity))
                        {
                            entity.MessagesFailed = modelEntity.MessagesFailed;
                            entity.ConnectionStatus = modelEntity.ConnectionStatus;
                            entity.ConnectionTransport = modelEntity.ConnectionTransport;
                            entity.BatteryLevel = modelEntity.BatteryLevel;
                            entity.BatteryCharging = modelEntity.BatteryCharging;
                            entity.LastSeen = modelEntity.LastSeen;
                            entity.LastSyncDate = modelEntity.LastSyncDate;
                            entity.LastUpdated = Today;
                        }
                    }
                }
                await _unitOfWork.CompleteAsync();
                return ApiResponseModel<string>.Success(GenericErrors.UpdateSuccess);
            }
            catch (Exception)
            {
                return ApiResponseModel<string>.Failure(GenericErrors.TransFailed);
            }
        }
    }
}
