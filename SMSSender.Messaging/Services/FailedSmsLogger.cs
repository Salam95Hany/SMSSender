using System.Text;
using SMSSender.Entities.Models.Messaging;
using SMSSender.Interfaces.Common;
using SMSSender.Interfaces.Repositories;

namespace SMSSender.Messaging.Services
{
    public class FailedSmsLogger : IFailedSmsLogger
    {
        private readonly IAppSettings _appSettings;
        private readonly IUnitOfWork _unitOfWork;

        public FailedSmsLogger(IAppSettings appSettings, IUnitOfWork unitOfWork)
        {
            _appSettings = appSettings;
            _unitOfWork = unitOfWork;
        }

        public async Task LogAsync(string rawMessage, string provider, string errorReason)
        {
            var createdAt = DateTime.Now;

            try
            {
                await _unitOfWork.Repository<FailedSmsLog>().AddAsync(new FailedSmsLog
                {
                    RawMessage = rawMessage ?? string.Empty,
                    Provider = provider ?? string.Empty,
                    ErrorReason = errorReason ?? string.Empty,
                    CreatedAt = createdAt
                });

                await _unitOfWork.CompleteAsync();
                return;
            }
            catch
            {
            }

            try
            {
                var failureSettings = _appSettings.MessageParsing?.FailedSms ?? new FailedSmsSettings();
                var rootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                var dateFolder = createdAt.ToString(string.IsNullOrWhiteSpace(failureSettings.DateFolderFormat) ? "yyyy-MM-dd" : failureSettings.DateFolderFormat);
                var targetDirectory = Path.Combine(
                    rootPath,
                    string.IsNullOrWhiteSpace(failureSettings.RelativeDirectory) ? "sms-failures" : failureSettings.RelativeDirectory,
                    dateFolder);

                Directory.CreateDirectory(targetDirectory);

                var filePath = Path.Combine(targetDirectory, $"sms_{Guid.NewGuid():N}.txt");
                var fileContent = new StringBuilder()
                    .AppendLine($"CreatedAt: {createdAt:O}")
                    .AppendLine($"Provider: {provider}")
                    .AppendLine($"ErrorReason: {errorReason}")
                    .AppendLine("RawMessage:")
                    .AppendLine(rawMessage)
                    .ToString();

                await File.WriteAllTextAsync(filePath, fileContent, Encoding.UTF8);
            }
            catch
            {
            }
        }
    }
}
