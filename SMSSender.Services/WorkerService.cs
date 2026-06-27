using SMSSender.Entities.Common;
using SMSSender.Entities.Contracts.DTOs.Worker;
using SMSSender.Entities.Models.DeviceConfig;
using SMSSender.Entities.Models.Global;
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
        private readonly ICurrentCustomerService _currentCustomerService;
        public WorkerService(IUnitOfWork unitOfWork, INotificationService notificationService, IHubNotificationService hubNotificationService, ICurrentCustomerService currentCustomerService)
        {
            _unitOfWork = unitOfWork;
            _notificationService = notificationService;
            _hubNotificationService = hubNotificationService;
            _currentCustomerService = currentCustomerService;
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
                _currentCustomerService.IsSystemJob = true;
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
                var CustomerId = Model.First().CustomerId;
                var BranchId = Model.First().BranchId;
                var Today = DateTime.Now.EgyptNow();
                var Entities = await _unitOfWork.Repository<DeviceHealth>().WhereAsync(x => x.CustomerId == CustomerId && x.BranchId == BranchId);
                var EntityLookup = Entities.ToDictionary(x => x.DeviceId);
                var NewDevices = new List<DeviceHealth>();

                foreach (var item in Model)
                {
                    if (EntityLookup.TryGetValue(item.DeviceId, out var entity))
                    {
                        entity.DeviceName = item.DeviceName;
                        entity.MessagesFailed = item.MessagesFailed;
                        entity.ConnectionStatus = item.ConnectionStatus;
                        entity.ConnectionTransport = item.ConnectionTransport;
                        entity.BatteryLevel = item.BatteryLevel;
                        entity.BatteryCharging = item.BatteryCharging;
                        entity.LastSeen = item.LastSeen.EgyptNow();
                        entity.LastSyncDate = item.LastSyncDate;
                        entity.LastUpdated = Today;
                    }
                    else
                    {
                        NewDevices.Add(new DeviceHealth
                        {
                            DeviceId = item.DeviceId,
                            CustomerId = CustomerId,
                            BranchId = BranchId,
                            DeviceName = item.DeviceName,
                            MessagesFailed = item.MessagesFailed,
                            ConnectionStatus = item.ConnectionStatus,
                            ConnectionTransport = item.ConnectionTransport,
                            BatteryLevel = item.BatteryLevel,
                            BatteryCharging = item.BatteryCharging,
                            LastSeen = item.LastSeen,
                            LastSyncDate = item.LastSyncDate,
                            LastUpdated = Today
                        });
                    }
                }

                if (NewDevices.Any())
                {
                    await _unitOfWork.Repository<DeviceHealth>().AddRangeAsync(NewDevices);
                }

                await _unitOfWork.CompleteAsync();

                return ApiResponseModel<string>.Success(GenericErrors.UpdateSuccess);
            }
            catch
            {
                return ApiResponseModel<string>.Failure(GenericErrors.TransFailed);
            }
        }

        public async Task<ApiResponseModel<string>> AddWorkerLogException(DeviceErrorLog Model)
        {
            try
            {
                await _unitOfWork.Repository<DeviceErrorLog>().AddAsync(Model);
                await _unitOfWork.CompleteAsync();

                return ApiResponseModel<string>.Success(GenericErrors.UpdateSuccess);
            }
            catch
            {
                return ApiResponseModel<string>.Failure(GenericErrors.TransFailed);
            }
        }
    }
}
