using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SMSSender.Interfaces;
using SMSSender.Interfaces.Hub;
using SMSSender.Messaging.FileLog;
using SMSSender.Messaging.Services;
using System.Diagnostics;

namespace SMSSender.Messaging.TaskQueue
{
    public class SmsProcessingWorker : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IBackgroundTaskQueue _queue;
        private readonly IFileLoggerService _fileLogger;

        public SmsProcessingWorker(IServiceScopeFactory scopeFactory, IBackgroundTaskQueue queue, IFileLoggerService fileLogger)
        {
            _scopeFactory = scopeFactory;
            _queue = queue;
            _fileLogger = fileLogger;
        }

        private static Stopwatch _stopwatch = new Stopwatch();
        private static int _processedCount = 0;
        private const int TargetMessages = 100;
        public static long LastElapsedMs => _stopwatch.ElapsedMilliseconds;
        public static int Processed => _processedCount;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _stopwatch.Start();
            while (!stoppingToken.IsCancellationRequested)
            {
                SmsMessagePure smsMessage = null;

                try
                {
                    smsMessage = await _queue.DequeueAsync(stoppingToken);

                    await using var scope = _scopeFactory.CreateAsyncScope();
                    var processingService = scope.ServiceProvider.GetRequiredService<IMessageProcessingService>();
                    var messageService = scope.ServiceProvider.GetRequiredService<IMessageService>();
                    var hubService = scope.ServiceProvider.GetRequiredService<IHubNotificationService>();

                    var acceptedMsg = messageService.GetMessageFiltered(smsMessage.ProviderStr, smsMessage.Message);
                    if (!acceptedMsg)
                        continue;

                    var process = await processingService.Process(smsMessage);
                    _processedCount++;
                    if (process.Success && process.TransactionId.HasValue)
                        await hubService.SendMessageAddedAsync();

                    if (_processedCount == TargetMessages)
                    {
                        _stopwatch.Stop();
                    }
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    await _fileLogger.LogError(ex);

                    if (smsMessage != null)
                    {
                        await _fileLogger.LogMessageData(smsMessage);
                    }
                }
            }
        }
    }
}
