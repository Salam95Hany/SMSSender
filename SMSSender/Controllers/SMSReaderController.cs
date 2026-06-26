using Microsoft.AspNetCore.Mvc;
using SMSSender.Entities.Models.Global;
using SMSSender.Interfaces;
using SMSSender.Interfaces.Common;
using SMSSender.Messaging;
using SMSSender.Messaging.Models;
using SMSSender.Messaging.Services;
using SMSSender.Messaging.TaskQueue;

namespace SMSSender.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SMSReaderController : ControllerBase
    {
        private readonly IAppSettings _appSettings;
        private readonly IBackgroundTaskQueue _taskQueue;
        private readonly IMessageService _messageService;
        private readonly ICurrentCustomerService _currentCustomerService;
        private readonly IMessageProcessingService _messageProcessingService;


        public SMSReaderController(IBackgroundTaskQueue taskQueue, IAppSettings appSettings, IMessageProcessingService messageProcessingService, ICurrentCustomerService currentCustomerService, IMessageService messageService)
        {
            _appSettings = appSettings;
            _taskQueue = taskQueue;
            _messageService = messageService;
            _currentCustomerService = currentCustomerService;
            _messageProcessingService = messageProcessingService;
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> IncomingMessage([FromBody] List<IncomingSmsParam> Model)
        {
            try
            {
                string secretKey = Request.Headers["Secret_Key"];

                if (secretKey != _appSettings.SecretKey)
                    return Unauthorized("unauthorized");

                var SmsNotExist = await _messageService.GetMessageNotExist(Model.Select(m => m.SmsGateId).ToList());

                var messages = Model.Where(i => SmsNotExist.Contains(i.SmsGateId)).Select(model => new SmsMessagePure
                {
                    SmsGateId = model.SmsGateId,
                    CustomerId = model.CustomerId,
                    BranchId = model.BranchId,
                    DeviceName = model.DeviceName,
                    PhoneNumber = model.ProviderPhone,
                    Message = model.Message,
                    ProviderStr = model.ProviderName,
                    CreatedAt = model.CreatedAt,
                    Sim = model.SimNumber
                });

                await _taskQueue.QueueRangeAsync(messages);

                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> IncomingRefreshInboxMessage([FromBody] List<IncomingSmsParam> Model)
        {
            try
            {
                string secretKey = Request.Headers["Secret_Key"];

                if (secretKey != _appSettings.SecretKey)
                    return Unauthorized("unauthorized");

                _currentCustomerService.CustomerId = Model.FirstOrDefault().CustomerId;
                _currentCustomerService.BranchId = Model.FirstOrDefault().BranchId;
                _currentCustomerService.IsAdmin = false;

                var SmsNotExist = await _messageService.GetMessageNotExist(Model.Select(m => m.SmsGateId).ToList());

                var messages = Model.Where(i => SmsNotExist.Contains(i.SmsGateId)).Select(model => new SmsMessagePure
                {
                    SmsGateId = model.SmsGateId,
                    CustomerId = model.CustomerId,
                    BranchId = model.BranchId,
                    DeviceName = model.DeviceName,
                    PhoneNumber = model.ProviderPhone,
                    Message = model.Message,
                    ProviderStr = model.ProviderName,
                    CreatedAt = model.CreatedAt,
                    Sim = model.SimNumber
                });

                foreach (var item in messages)
                    await _messageProcessingService.Process(item);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }
}
