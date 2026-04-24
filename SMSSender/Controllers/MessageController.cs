using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SMSSender.Interfaces;
using SMSSender.Interfaces.Common;
using SMSSender.Messaging;
using SMSSender.Messaging.Models;
using SMSSender.Messaging.Services;
using System.Collections.Concurrent;
using System.Text;

namespace SMSSender.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IAppSettings _appSettings;
        private readonly IMessageService _messageService;
        private readonly IMessageProcessingService _processingService;
        private static readonly ConcurrentBag<HttpResponse> _clients = new();


        public MessageController(IMessageProcessingService processingService, IAppSettings appSettings, IMessageService messageService)
        {
            _appSettings = appSettings;
            _messageService = messageService;
            _processingService = processingService;
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> IncomingMessage([FromBody] IncomingSmsParam model)
        {
            try
            {
                string secretKey = Request.Headers["User-Agent"];
                if (secretKey != _appSettings.SecretKey)
                    return Unauthorized();

                var AcceptedMsg = _messageService.GetMessageFiltered(model.From, model.Text);
                if (!AcceptedMsg)
                    return Ok();


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

                var Process = await _processingService.Process(smsMessage);
                if (Process)
                {
                    await BroadcastAsync("Message_Added");
                    return Ok();
                }
                else
                    return BadRequest();

            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("stream")]
        public async Task Stream()
        {
            Response.Headers.Add("Cache-Control", "no-cache");
            Response.Headers.Add("Content-Type", "text/event-stream");
            _clients.Add(Response);
            await Response.Body.FlushAsync();
            await Task.Run(() => HttpContext.RequestAborted.WaitHandle.WaitOne());
            _clients.TryTake(out _);
        }

        public static async Task BroadcastAsync(string msg)
        {
            foreach (var client in _clients)
            {
                try
                {
                    var data = $"data: {msg}\n\n";
                    var bytes = Encoding.UTF8.GetBytes(data);

                    await client.Body.WriteAsync(bytes);
                    await client.Body.FlushAsync();
                }
                catch
                {

                }
            }
        }
    }
}
