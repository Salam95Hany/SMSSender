using Newtonsoft.Json;
using SMSSender.Messaging.Services;
using System.Text;

namespace SMSSender.Messaging.FileLog
{
    public class FileLoggerService : IFileLoggerService
    {
        private static readonly SemaphoreSlim _fileLock = new(1, 1);

        public async Task LogMessageData(object model)
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
                .AppendLine($"InputParam: {JsonConvert.SerializeObject(model, Formatting.Indented)}")
                .ToString();

            await _fileLock.WaitAsync();

            try
            {
                await File.AppendAllTextAsync(filePath, fileContent, Encoding.UTF8);
            }
            finally
            {
                _fileLock.Release();
            }
        }

        public async Task LogError(Exception ex)
        {
            var createdAt = DateTime.UtcNow.EgyptNow();
            var rootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var dateFolder = createdAt.ToString("yyyy-MM-dd");
            var targetDirectory = Path.Combine(rootPath, "sms-errors", dateFolder);

            if (!Directory.Exists(targetDirectory))
                Directory.CreateDirectory(targetDirectory);

            var filePath = Path.Combine(targetDirectory, $"errors_{createdAt:yyyy-MM-dd}.txt");

            var sb = new StringBuilder()
                .AppendLine()
                .AppendLine("====================================")
                .AppendLine($"CreatedAt: {createdAt:O}");

            int level = 0;
            Exception? current = ex;

            while (current != null)
            {
                sb.AppendLine()
                  .AppendLine($"Exception Level: {level}")
                  .AppendLine($"Type: {current.GetType().FullName}")
                  .AppendLine($"Message: {current.Message}")
                  .AppendLine($"StackTrace: {current.StackTrace}");

                current = current.InnerException;
                level++;
            }

            await _fileLock.WaitAsync();

            try
            {
                await File.AppendAllTextAsync(filePath, sb.ToString(), Encoding.UTF8);
            }
            finally
            {
                _fileLock.Release();
            }
        }
    }
}
