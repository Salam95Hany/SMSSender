using System.Text;
using Newtonsoft.Json;
using SMSSender.Entities.Models.Messaging;
using SMSSender.Interfaces.Common;
using SMSSender.Interfaces.Repositories;
using SMSSender.Messaging.Models;

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

        public async Task LogAsync(SmsMessagePure model, string errorReason, bool IsCorrectionProcess = false)
        {
            var createdAt = DateTime.Now;

            try
            {
                await _unitOfWork.Repository<SmsMessageLog>().AddAsync(new SmsMessageLog
                {
                    TransactionId = Guid.NewGuid(),
                    Message = model.Message ?? string.Empty,
                    ErrorMessage = errorReason,
                    MsgStatus = IsCorrectionProcess ?  MsgStatus.CorrectionProcess.ToString() : MsgStatus.CorrectionProcess.ToString(),
                    Provider = model.ProviderStr ?? string.Empty,
                    ProviderName = model.DeviceName ?? string.Empty,
                    ProviderPhone = model.PhoneNumber ?? string.Empty,
                    SentStamp = model.SentStamp ?? string.Empty,
                    ReceivedStamp = model.ReceivedStamp ?? string.Empty,
                    Sim = model.Sim ?? string.Empty,
                    CreatedDate = createdAt,
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

                var filePath = Path.Combine(targetDirectory, $"sms_{createdAt:HH-mm-ss-fff}.txt");
                var fileContent = new StringBuilder()
                    .AppendLine($"CreatedAt: {createdAt:O}")
                    .AppendLine($"ErrorReason: {errorReason}")
                    .AppendLine($"InputParam: {JsonConvert.SerializeObject(model, Formatting.Indented)}")
                    .ToString();

                await File.WriteAllTextAsync(filePath, fileContent, Encoding.UTF8);
            }
            catch
            {
            }
        }
    }
}
