using Microsoft.AspNetCore.Mvc;
using SMSSender.Entities.Common;
using SMSSender.Entities.Contracts.DTOs;
using SMSSender.Entities.Models.DeviceConfig;
using SMSSender.Interfaces;

namespace SMSSender.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        private readonly IDeviceService _deviceService;
        public DeviceController(IDeviceService deviceService)
        {
            _deviceService = deviceService;
        }

        [HttpGet("GetCustomerDevices")]
        public async Task<List<Device>> GetCustomerDevices()
        {
            var results = await _deviceService.GetCustomerDevices();
            return results;
        }

        [HttpGet("GetCustomerDeviceChangeLog")]
        public async Task<List<DeviceChangeLog>> GetCustomerDeviceChangeLog()
        {
            var results = await _deviceService.GetCustomerDeviceChangeLog();
            return results;
        }

        [HttpGet("GetCustomerDeviceHealthStatus")]
        public async Task<List<DeviceHealth>> GetCustomerDeviceHealthStatus()
        {
            var results = await _deviceService.GetCustomerDeviceHealthStatus();
            return results;
        }

        [HttpGet("GetCustomerDeviceRefreshInbox")]
        public async Task<List<DeviceRefreshInboxDto>> GetCustomerDeviceRefreshInbox()
        {
            var results = await _deviceService.GetCustomerDeviceRefreshInbox();
            return results;
        }

        [HttpPost("AddNewDevice")]
        public async Task<ApiResponseModel<string>> AddNewDevice(Device Model)
        {
            var results = await _deviceService.AddNewDevice(Model);
            return results;
        }

        [HttpPost("UpdateDevice")]
        public async Task<ApiResponseModel<string>> UpdateDevice(Device Model)
        {
            var results = await _deviceService.UpdateDevice(Model);
            return results;
        }

        [HttpGet("DeleteDevice")]
        public async Task<ApiResponseModel<string>> DeleteDevice(int DeviceId, bool IsActive)
        {
            var results = await _deviceService.DeleteDevice(DeviceId, IsActive);
            return results;
        }

        [HttpPost("RefreshDeviceInbox")]
        public async Task<ApiResponseModel<string>> RefreshDeviceInbox(DeviceRefreshInbox Model)
        {
            var results = await _deviceService.RefreshDeviceInbox(Model);
            return results;
        }
    }
}
