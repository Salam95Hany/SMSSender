using Microsoft.AspNetCore.Mvc;
using SMSSender.Entities.Contracts.DTOs.Worker;
using SMSSender.Interfaces;
using SMSSender.Interfaces.Common;
using SMSSender.Messaging.FileLog;

namespace SMSSender.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkerController : ControllerBase
    {
        private readonly IWorkerService _workerService;
        private readonly IAppSettings _appSettings;
        private readonly IFileLoggerService _fileLogger;

        public WorkerController(IWorkerService workerService, IAppSettings appSettings, IFileLoggerService fileLogger)
        {
            _workerService = workerService;
            _appSettings = appSettings;
            _fileLogger = fileLogger;
        }

        [HttpGet("device")]
        public async Task<IActionResult> GetAllDevices(Guid CustomerId, int BranchId, int Version)
        {
            try
            {
                string secretKey = Request.Headers["Secret_Key"];

                if (secretKey != _appSettings.SecretKey)
                    return Unauthorized("unauthorized");

                var Results = await _workerService.GetAllDevices(CustomerId, BranchId, Version);

                return Ok(Results);
            }
            catch (Exception ex)
            {
                await _fileLogger.LogError(ex);
                return BadRequest();
            }
        }

        [HttpPost("health")]
        public async Task<IActionResult> UpdateDeviceHealth([FromBody] List<DeviceHealthDetailsRequest> Model)
        {
            try
            {
                string secretKey = Request.Headers["Secret_Key"];

                if (secretKey != _appSettings.SecretKey)
                    return Unauthorized("unauthorized");

                await _workerService.UpdateDeviceHealth(Model);

                return Ok();
            }
            catch (Exception ex)
            {
                await _fileLogger.LogError(ex);
                return BadRequest();
            }
        }

        [HttpGet("inbox")]
        public async Task<IActionResult> GetDeviceRefreshInboxSetting(Guid CustomerId, int BranchId)
        {
            try
            {
                string secretKey = Request.Headers["Secret_Key"];

                if (secretKey != _appSettings.SecretKey)
                    return Unauthorized("unauthorized");

                var Results = await _workerService.GetDeviceRefreshInboxSetting(CustomerId, BranchId);

                return Ok(Results);
            }
            catch (Exception ex)
            {
                await _fileLogger.LogError(ex);
                return BadRequest();
            }
        }

        [HttpGet("inbox/status")]
        public async Task<IActionResult> UpdateDeviceRefreshInboxStatus(Guid CustomerId, int BranchId, string DeviceId)
        {
            try
            {
                string secretKey = Request.Headers["Secret_Key"];

                if (secretKey != _appSettings.SecretKey)
                    return Unauthorized("unauthorized");

                var Results = await _workerService.UpdateDeviceRefreshInboxStatus(CustomerId, BranchId, DeviceId);

                return Ok(Results);
            }
            catch (Exception ex)
            {
                await _fileLogger.LogError(ex);
                return BadRequest();
            }
        } 
    }
}
