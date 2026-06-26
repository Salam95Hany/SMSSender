using SMSSender.Entities.Common;
using SMSSender.Entities.Contracts.DTOs.Worker;
using SMSSender.Entities.Models.DeviceConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSSender.Interfaces
{
    public interface IWorkerService
    {
        Task<DeviceWorkrtDto> GetAllDevices(Guid CustomerId, int BranchId, int Version);
        Task<RefreshInboxDto> GetDeviceRefreshInboxSetting(Guid CustomerId, int BranchId);
        Task<bool> UpdateDeviceRefreshInboxStatus(Guid CustomerId, int BranchId, string DeviceId);
        Task<ApiResponseModel<string>> UpdateDeviceHealth(List<DeviceHealthDetailsRequest> Model);
        Task<ApiResponseModel<string>> AddWorkerLogException(DeviceErrorLog Model);
    }
}
