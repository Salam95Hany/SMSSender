using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
    public class SMSReaderController : ControllerBase
    {
        private readonly IAppSettings _appSettings;
        private readonly IMessageService _messageService;
        private readonly IMessageProcessingService _processingService;
        private static readonly ConcurrentDictionary<string, HttpResponse> _clients = new();


        public SMSReaderController(IMessageProcessingService processingService, IAppSettings appSettings, IMessageService messageService)
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
                
                //await LogMessageData(smsMessage, secretKey);
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
                if (Process.Success)
                {
                    await BroadcastAsync("Message_Added", Process.TransactionId.Value);
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
            Response.Headers.Add("Content-Type", "text/event-stream");
            Response.Headers.Add("Cache-Control", "no-cache");
            Response.Headers.Add("Connection", "keep-alive");

            var clientId = Guid.NewGuid().ToString();
            _clients.TryAdd(clientId, Response);

            try
            {
                while (!HttpContext.RequestAborted.IsCancellationRequested)
                {
                    await Response.WriteAsync(": keep-alive\n\n");
                    await Response.Body.FlushAsync();
                    await Task.Delay(15000, HttpContext.RequestAborted);
                }
            }
            catch (TaskCanceledException)
            {
                // client disconnected
            }
            finally
            {
                _clients.TryRemove(clientId, out _);
            }
        }

        public static async Task BroadcastAsync(string msg, Guid transactionId)
        {
            var payload = JsonConvert.SerializeObject(new
            {
                message = msg,
                transactionId
            });

            var data = $"data: {payload}\n\n";
            var bytes = Encoding.UTF8.GetBytes(data);

            var deadClients = new List<string>();

            foreach (var client in _clients)
            {
                try
                {
                    await client.Value.Body.WriteAsync(bytes);
                    await client.Value.Body.FlushAsync();
                }
                catch
                {
                    deadClients.Add(client.Key);
                }
            }

            foreach (var dead in deadClients)
            {
                _clients.TryRemove(dead, out _);
            }
        }

        public async Task LogMessageData(SmsMessagePure Model, string secretKey)
        {
            var createdAt = DateTime.Now;
            var rootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var dateFolder = createdAt.ToString("yyyy-MM-dd");
            var targetDirectory = Path.Combine(rootPath, "sms-log", dateFolder);
            if (!Directory.Exists(targetDirectory))
                Directory.CreateDirectory(targetDirectory);
            var filePath = Path.Combine(targetDirectory, $"sms_{createdAt:HH-mm-ss-fff}.txt");
            var fileContent = new StringBuilder()
                .AppendLine($"CreatedAt: {createdAt:O}")
                .AppendLine($"SecretKey: {secretKey}")
                .AppendLine($"InputParam: {JsonConvert.SerializeObject(Model, Formatting.Indented)}")
                .ToString();

            await System.IO.File.WriteAllTextAsync(filePath, fileContent, Encoding.UTF8);
        }
    }
}
