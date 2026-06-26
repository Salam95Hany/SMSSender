using Microsoft.EntityFrameworkCore;
using SMSSender.Entities.Common;
using SMSSender.Entities.Contracts.DTOs;
using SMSSender.Entities.Models.Config;
using SMSSender.Entities.Models.DeviceConfig;
using SMSSender.Entities.Models.Global;
using SMSSender.Interfaces;
using SMSSender.Interfaces.Repositories;
using SMSSender.Services.Common;

namespace SMSSender.Services
{
    public class DeviceService : IDeviceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentCustomerService _currentCustomerService;
        private Guid CustomerId;
        public DeviceService(IUnitOfWork unitOfWork, ICurrentCustomerService currentCustomerService)
        {
            _unitOfWork = unitOfWork;
            _currentCustomerService = currentCustomerService;
            CustomerId = _currentCustomerService.CustomerId;
        }

        public async Task<List<Device>> GetCustomerDevices()
        {
            var Data = await (
            from device in _unitOfWork.Repository<Device>().GetAllAsQueryable()
            join branch in _unitOfWork.Repository<Branch>().GetAllAsQueryable()
            on device.BranchId equals branch.BranchId
            where device.CustomerId == CustomerId
            select new Device
            {
                DeviceId = device.DeviceId,
                DeviceUniqueId = device.DeviceUniqueId,
                BranchName = branch.Name,
                BranchId = device.BranchId,
                DeviceName = device.DeviceName,
                BaseUrl = device.BaseUrl,
                Username = device.Username,
                Password = device.Password,
                Sim1Number = device.Sim1Number,
                Sim2Number = device.Sim2Number,
                Sim1Name = device.Sim1Name,
                Sim2Name = device.Sim2Name,
                IsActive = device.IsActive,
                CreatedAt = device.CreatedAt,
                UpdatedAt = device.UpdatedAt
            }).ToListAsync();

            return Data;
        }

        public async Task<List<DeviceChangeLog>> GetCustomerDeviceChangeLog()
        {
            var LastVersion = await _unitOfWork.Repository<DeviceSyncVersion>().FirstOrDefaultAsync(i => i.CustomerId == CustomerId);
            var Data = await (
            from log in _unitOfWork.Repository<DeviceChangeLog>().GetAllAsQueryable()
            join device in _unitOfWork.Repository<Device>().GetAllAsQueryable()
            on log.DeviceId equals device.DeviceUniqueId
            where log.CustomerId == CustomerId
            select new DeviceChangeLog
            {
                DeviceChangeLogId = log.DeviceChangeLogId,
                DeviceId = log.DeviceId,
                DeviceName = device.DeviceName,
                Version = log.Version,
                Action = log.Action,
                FieldKey = log.FieldKey,
                FieldValue = log.FieldValue,
                CreatedAt = log.CreatedAt,
                LastVersion = LastVersion.CurrentVersion
            }).ToListAsync();
            return Data;
        }

        public async Task<List<DeviceHealth>> GetCustomerDeviceHealthStatus()
        {
            var Data = await (
            from deviceHealth in _unitOfWork.Repository<DeviceHealth>().GetAllAsQueryable()
            join branch in _unitOfWork.Repository<Branch>().GetAllAsQueryable()
            on deviceHealth.BranchId equals branch.BranchId
            where deviceHealth.CustomerId == CustomerId
            select new DeviceHealth
            {
                DeviceId = deviceHealth.DeviceId,
                BranchName = branch.Name,
                DeviceName = deviceHealth.DeviceName,
                MessagesFailed = deviceHealth.MessagesFailed,
                ConnectionStatus = deviceHealth.ConnectionStatus,
                ConnectionTransport = deviceHealth.ConnectionTransport,
                BatteryLevel = deviceHealth.BatteryLevel,
                BatteryCharging = deviceHealth.BatteryCharging,
                LastSeen = deviceHealth.LastSeen,
                LastSyncDate = deviceHealth.LastSyncDate,
                LastUpdated = deviceHealth.LastUpdated
            }).ToListAsync();

            return Data;
            return Data;
        }

        public async Task<List<DeviceRefreshInboxDto>> GetCustomerDeviceRefreshInbox()
        {
            var Data = await (
            from device in _unitOfWork.Repository<Device>().GetAllAsQueryable()
            join refresh in _unitOfWork.Repository<DeviceRefreshInbox>().GetAllAsQueryable()
            on device.DeviceUniqueId equals refresh.DeviceId into refreshGroup
            from refresh in refreshGroup.DefaultIfEmpty()
            join branch in _unitOfWork.Repository<Branch>().GetAllAsQueryable()
            on device.BranchId equals branch.BranchId
            where device.CustomerId == CustomerId
            select new DeviceRefreshInboxDto
            {
                DeviceRefreshInboxId = refresh.DeviceRefreshInboxId,
                DeviceId = device.DeviceUniqueId,
                BranchId = device.BranchId.Value,
                DeviceName = device.DeviceName,
                BranchName = branch.Name,
                IsActive = device.IsActive,
                RefreshStatus = refresh.RefreshStatus,
                From = refresh.From,
                To = refresh.To,
                LastUpdate = refresh.LastUpdate
            }).ToListAsync();

            return Data;
        }


        public async Task<ApiResponseModel<string>> AddNewDevice(Device Model)
        {
            try
            {
                var DeviceIsExist = await _unitOfWork.Repository<Device>().AnyAsync(i =>
                i.CustomerId == CustomerId &&
                i.BranchId == Model.BranchId &&
                (
                    i.DeviceUniqueId == Model.DeviceUniqueId ||
                    (!string.IsNullOrWhiteSpace(Model.Sim1Number) && i.Sim1Number == Model.Sim1Number) ||
                    (!string.IsNullOrWhiteSpace(Model.Sim1Name) && i.Sim1Name == Model.Sim1Name) ||
                    (!string.IsNullOrWhiteSpace(Model.Sim2Number) && i.Sim2Number == Model.Sim2Number) ||
                    (!string.IsNullOrWhiteSpace(Model.Sim2Name) && i.Sim2Name == Model.Sim2Name)
                ));
                if (DeviceIsExist)
                    return ApiResponseModel<string>.Failure(GenericErrors.DeviceIsExist);

                var LastVersion = await _unitOfWork.Repository<DeviceSyncVersion>().FirstOrDefaultAsync(i => i.CustomerId == CustomerId);
                Model.CustomerId = CustomerId;
                Model.BranchId = Model.BranchId;
                Model.CreatedAt = DateTime.Now.EgyptNow();

                var Log = new DeviceChangeLog
                {
                    DeviceId = Model.DeviceUniqueId,
                    CustomerId = CustomerId,
                    BranchId = Model.BranchId.Value,
                    Version = LastVersion.CurrentVersion + 1,
                    Action = ChangeLogAction.Created,
                    CreatedAt = DateTime.Now.EgyptNow()
                };

                LastVersion.CurrentVersion = Log.Version;
                await _unitOfWork.Repository<Device>().AddAsync(Model);
                await _unitOfWork.Repository<DeviceChangeLog>().AddAsync(Log);
                await _unitOfWork.CompleteAsync();

                return ApiResponseModel<string>.Success(GenericErrors.AddSuccess);
            }
            catch (Exception)
            {
                return ApiResponseModel<string>.Failure(GenericErrors.TransFailed);
            }
        }

        public async Task<ApiResponseModel<string>> UpdateDevice(Device Model)
        {
            try
            {
                var Entity = await _unitOfWork.Repository<Device>().FirstOrDefaultAsync(i => i.CustomerId == CustomerId && i.DeviceId == Model.DeviceId);

                if (Entity == null)
                    return ApiResponseModel<string>.Failure(GenericErrors.NotFound);

                var DeviceIsExist = await _unitOfWork.Repository<Device>().AnyAsync(i =>
                i.CustomerId == CustomerId &&
                i.BranchId == Model.BranchId &&
                i.DeviceId != Model.DeviceId &&
                i.DeviceId != Model.DeviceId &&
                (
                    i.DeviceUniqueId == Model.DeviceUniqueId ||
                    (!string.IsNullOrWhiteSpace(Model.Sim1Number) && i.Sim1Number == Model.Sim1Number) ||
                    (!string.IsNullOrWhiteSpace(Model.Sim1Name) && i.Sim1Name == Model.Sim1Name) ||
                    (!string.IsNullOrWhiteSpace(Model.Sim2Number) && i.Sim2Number == Model.Sim2Number) ||
                    (!string.IsNullOrWhiteSpace(Model.Sim2Name) && i.Sim2Name == Model.Sim2Name)
                ));

                if (DeviceIsExist)
                    return ApiResponseModel<string>.Failure(GenericErrors.DeviceIsExist);


                var LastVersion = await _unitOfWork.Repository<DeviceSyncVersion>().FirstOrDefaultAsync(i => i.CustomerId == CustomerId && i.BranchId == Model.BranchId);
                int NewVersion = LastVersion.CurrentVersion;
                var logs = new List<DeviceChangeLog>();
                var properties = typeof(Device).GetProperties();


                foreach (var property in properties)
                {
                    if (property.Name == nameof(Device.DeviceId) ||
                        property.Name == nameof(Device.CustomerId) ||
                        property.Name == nameof(Device.BranchId) ||
                        property.Name == nameof(Device.CreatedAt) ||
                        property.Name == nameof(Device.UpdatedAt) ||
                        property.Name == nameof(Device.IsActive) ||
                        property.Name == nameof(Device.BranchName))
                        continue;


                    var OldValue = property.GetValue(Entity)?.ToString();
                    var NewValue = property.GetValue(Model)?.ToString();


                    if (OldValue != NewValue)
                    {
                        NewVersion++;
                        logs.Add(new DeviceChangeLog
                        {
                            DeviceId = Entity.DeviceUniqueId,
                            CustomerId = CustomerId,
                            BranchId = Model.BranchId.Value,
                            Version = NewVersion,
                            Action = ChangeLogAction.Updated,
                            FieldKey = property.Name,
                            FieldValue = NewValue,
                            CreatedAt = DateTime.Now.EgyptNow()
                        });
                    }
                }

                Entity.DeviceUniqueId = Model.DeviceUniqueId;
                Entity.DeviceName = Model.DeviceName;
                Entity.BaseUrl = Model.BaseUrl;
                Entity.Username = Model.Username;
                Entity.Password = Model.Password;
                Entity.Sim1Number = Model.Sim1Number;
                Entity.Sim2Number = Model.Sim2Number;
                Entity.Sim1Name = Model.Sim1Name;
                Entity.Sim2Name = Model.Sim2Name;
                Entity.UpdatedAt = DateTime.Now.EgyptNow();


                if (logs.Any())
                    await _unitOfWork.Repository<DeviceChangeLog>().AddRangeAsync(logs);

                LastVersion.CurrentVersion = NewVersion;
                await _unitOfWork.CompleteAsync();

                return ApiResponseModel<string>.Success(GenericErrors.UpdateSuccess);
            }
            catch (Exception)
            {
                return ApiResponseModel<string>.Failure(GenericErrors.TransFailed);
            }
        }

        public async Task<ApiResponseModel<string>> DeleteDevice(int DeviceId, bool IsActive)
        {
            try
            {
                var Entity = await _unitOfWork.Repository<Device>().FirstOrDefaultAsync(i => i.CustomerId == CustomerId && i.DeviceId == DeviceId);
                if (Entity == null)
                    return ApiResponseModel<string>.Failure(GenericErrors.NotFound);

                var LastVersion = await _unitOfWork.Repository<DeviceSyncVersion>().FirstOrDefaultAsync(i => i.CustomerId == CustomerId && i.BranchId == Entity.BranchId);
                Entity.IsActive = IsActive;

                var Log = new DeviceChangeLog
                {
                    DeviceId = Entity.DeviceUniqueId,
                    CustomerId = CustomerId,
                    BranchId = Entity.BranchId.Value,
                    Version = LastVersion.CurrentVersion + 1,
                    Action = ChangeLogAction.Deleted,
                    FieldKey = "IsActive",
                    FieldValue = IsActive.ToString(),
                    CreatedAt = DateTime.Now.EgyptNow()
                };

                LastVersion.CurrentVersion = Log.Version;
                await _unitOfWork.Repository<DeviceChangeLog>().AddAsync(Log);
                await _unitOfWork.CompleteAsync();

                return ApiResponseModel<string>.Success(GenericErrors.DeleteSuccess);
            }
            catch (Exception)
            {
                return ApiResponseModel<string>.Failure(GenericErrors.TransFailed);
            }
        }

        public async Task<ApiResponseModel<string>> RefreshDeviceInbox(DeviceRefreshInbox Model)
        {
            try
            {
                var Entity = await _unitOfWork.Repository<DeviceRefreshInbox>().FirstOrDefaultAsync(i => i.CustomerId == CustomerId && i.BranchId == Model.BranchId && i.DeviceId == Model.DeviceId);
                if (Entity == null)
                {
                    Entity = new DeviceRefreshInbox
                    {
                        DeviceId = Model.DeviceId,
                        CustomerId = CustomerId,
                        BranchId = _currentCustomerService.BranchId,
                        RefreshStatus = InboxRefreshStatus.Pending,
                        From = Model.From?.Date,
                        To = Model.To?.AddDays(1).AddTicks(-1),
                        CreatedAt = DateTime.Now.EgyptNow(),
                        LastUpdate = DateTime.Now.EgyptNow()
                    };

                    await _unitOfWork.Repository<DeviceRefreshInbox>().AddAsync(Entity);
                }
                else
                {
                    Entity.RefreshStatus = InboxRefreshStatus.Pending;
                    Entity.From = Model.From?.Date;
                    Entity.To = Model.To?.AddDays(1).AddTicks(-1);
                    Entity.LastUpdate = DateTime.Now.EgyptNow();
                }

                await _unitOfWork.CompleteAsync();
                return ApiResponseModel<string>.Success(GenericErrors.AddSuccess);
            }
            catch (Exception)
            {
                return ApiResponseModel<string>.Failure(GenericErrors.TransFailed);
            }
        }
    }
}
