using Microsoft.AspNetCore.Mvc;
using SMSSender.Entities.Models.Global;
using SMSSender.Interfaces.Common;
using SMSSender.Messaging;
using SMSSender.Messaging.FileLog;
using SMSSender.Messaging.Models;
using SMSSender.Messaging.TaskQueue;

namespace SMSSender.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SMSReaderController : ControllerBase
    {
        private readonly IAppSettings _appSettings;
        private readonly IBackgroundTaskQueue _taskQueue;
        private readonly ICurrentCustomerService _currentCustomerService;
        private readonly IFileLoggerService _fileLogger;

        public SMSReaderController(IBackgroundTaskQueue taskQueue, IAppSettings appSettings, ICurrentCustomerService currentCustomerService, IFileLoggerService fileLogger)
        {
            _appSettings = appSettings;
            _taskQueue = taskQueue;
            _currentCustomerService = currentCustomerService;
            _fileLogger = fileLogger;
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> IncomingMessage([FromBody] IncomingSmsParam model)
        {
            try
            {
                string secretKey = Request.Headers["User-Agent"];

                if (secretKey != _appSettings.SecretKey)
                    return Unauthorized("unauthorized");

                string deviceName = Request.Headers["Device-Name"];
                string phoneNumber = Request.Headers["Phone-Number"];

                var smsMessage = new SmsMessagePure
                {
                    CustomerId = _currentCustomerService.CustomerId,
                    BranchId = _currentCustomerService.BranchId,
                    DeviceName = deviceName,
                    PhoneNumber = phoneNumber,
                    Message = model.Text,
                    ProviderStr = model.From,
                    ReceivedStamp = model.ReceivedStamp,
                    SentStamp = model.SentStamp,
                    Sim = model.Sim
                };
                //await _fileLogger.LogMessageData(smsMessage);
                await _taskQueue.QueueAsync(smsMessage);

                return Content("success", "text/plain");
            }
            catch
            {
                return BadRequest("error");
            }
        }
    }
}
