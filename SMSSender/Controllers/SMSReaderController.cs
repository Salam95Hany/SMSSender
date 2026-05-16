using Microsoft.AspNetCore.Mvc;
using SMSSender.Interfaces.Common;
using SMSSender.Messaging;
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

        public SMSReaderController(IBackgroundTaskQueue taskQueue, IAppSettings appSettings)
        {
            _appSettings = appSettings;
            _taskQueue = taskQueue;
        }

        [HttpGet("stats")]
        public IActionResult GetStats()
        {
            return Ok(new
            {
                Processed = SmsProcessingWorker.Processed,
                TimeMs = SmsProcessingWorker.LastElapsedMs
            });
        }

        [HttpGet("test-queue-parallel")]
        public async Task<IActionResult> TestQueueParallel()
        {
            var tasks = new List<Task>();

            for (int i = 1; i <= 500; i++)
            {
                int index = i;

                tasks.Add(Task.Run(async () =>
                {
                    var smsMessage = new SmsMessagePure
                    {
                        DeviceName = "TestDevice",
                        PhoneNumber = "01124564843",
                        Message = $"تم تحويل 490 جنيه لرقم 01030579175 مصاريف الخدمة 1 جنيه رصيد حسابك فى فودافون كاش الحالي 50361.08. تاريخ العملية: 00:43 26-05-15 رقم العملية: 020001129645 مع كل معاملة بفودافون كاش هتزود فرصتك انك تكسب جنيه دهب لست الحبايب ,حول، اشحن،جدد باقتك، وادفع فواتيرك علشان تزود فرصتك من خلال http://vf.eg/vfcash",
                        ProviderStr = "VF-Cash",
                        ReceivedStamp = "1778795024000",
                        SentStamp = "1778795027999",
                        Sim = "sim1"
                    };

                    await _taskQueue.QueueAsync(smsMessage);
                }));
            }

            await Task.WhenAll(tasks);

            return Ok("100 messages queued in parallel");
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
                    DeviceName = deviceName,
                    PhoneNumber = phoneNumber,
                    Message = model.Text,
                    ProviderStr = model.From,
                    ReceivedStamp = model.ReceivedStamp,
                    SentStamp = model.SentStamp,
                    Sim = model.Sim
                };

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
