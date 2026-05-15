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
        public IActionResult IncomingMessage([FromBody] IncomingSmsParam model)
        {
            try
            {
                string secretKey = Request.Headers["User-Agent"];

                if (secretKey != _appSettings.SecretKey)
                    return Unauthorized("unauthorized");

                string deviceName = "ميار 631";
                string phoneNumber = "01030972631";

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

                _ = Task.Run(async () =>
                {
                    try
                    {
                        if (smsMessage.ProviderStr == "ALEXBANK")
                            await LogMessageData(smsMessage, secretKey);

                        var acceptedMsg = _messageService.GetMessageFiltered(smsMessage.ProviderStr, smsMessage.Message);
                        if (!acceptedMsg)
                            return;

                        var process = await _processingService.Process(smsMessage);

                        if (process.Success && process.TransactionId.HasValue)
                            await BroadcastAsync("Message_Added", process.TransactionId.Value);
                    }
                    catch (Exception ex)
                    {
                        await LogError(ex);
                    }
                });

                return Content("success", "text/plain");
            }
            catch (Exception ex)
            {
                _ = LogError(ex);
                return BadRequest("error");
            }
        }

        [HttpGet("stream")]
        public async Task Stream()
        {
            Response.Headers["Content-Type"] = "text/event-stream";
            Response.Headers["Cache-Control"] = "no-cache";
            Response.Headers["Connection"] = "keep-alive";

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
            catch
            {
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
                    await client.Value.Body.WriteAsync(bytes, 0, bytes.Length);
                    await client.Value.Body.FlushAsync();
                }
                catch
                {
                    deadClients.Add(client.Key);
                }
            }

            foreach (var dead in deadClients)
                _clients.TryRemove(dead, out _);
        }

        public async Task LogMessageData(SmsMessagePure model, string secretKey)
        {
            var createdAt = DateTime.Now;
            var rootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var dateFolder = createdAt.ToString("yyyy-MM-dd");
            var targetDirectory = Path.Combine(rootPath, "sms-log", dateFolder);

            if (!Directory.Exists(targetDirectory))
                Directory.CreateDirectory(targetDirectory);

            var filePath = Path.Combine(targetDirectory, $"sms_{createdAt:yyyy-MM-dd}.txt");

            var fileContent = new StringBuilder()
                .AppendLine()
                .AppendLine("====================================")
                .AppendLine($"CreatedAt: {createdAt:O}")
                .AppendLine($"SecretKey: {secretKey}")
                .AppendLine($"InputParam: {JsonConvert.SerializeObject(model, Formatting.Indented)}")
                .ToString();

            await System.IO.File.AppendAllTextAsync(filePath, fileContent, Encoding.UTF8);
        }

        public async Task LogError(Exception ex)
        {
            var rootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "logs");

            if (!Directory.Exists(rootPath))
                Directory.CreateDirectory(rootPath);

            var filePath = Path.Combine(rootPath, $"error_{DateTime.Now:yyyy-MM-dd}.txt");
            await System.IO.File.AppendAllTextAsync(filePath, $"{DateTime.Now:O}\n{ex}\n----------------------\n");
        }
    }
}
