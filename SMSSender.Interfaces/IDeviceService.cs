using SMSSender.Entities.Common;
using SMSSender.Entities.Contracts.DTOs;
using SMSSender.Entities.Contracts.DTOs.Worker;
using SMSSender.Entities.Models.DeviceConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSSender.Interfaces
{
    public interface IDeviceService
    {
        Task<List<Device>> GetCustomerDevices();
        Task<List<DeviceChangeLog>> GetCustomerDeviceChangeLog();
        Task<List<DeviceHealth>> GetCustomerDeviceHealthStatus();
        Task<List<DeviceRefreshInboxDto>> GetCustomerDeviceRefreshInbox();
        Task<ApiResponseModel<string>> AddNewDevice(Device Model);
        Task<ApiResponseModel<string>> UpdateDevice(Device Model);
        Task<ApiResponseModel<string>> DeleteDevice(int DeviceId, bool IsActive);
        Task<ApiResponseModel<string>> RefreshDeviceInbox(DeviceRefreshInbox Model);
    }
}
