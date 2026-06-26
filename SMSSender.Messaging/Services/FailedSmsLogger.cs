using System.Text;
using Newtonsoft.Json;
using SMSSender.Entities.Models.Messaging;
using SMSSender.Interfaces.Repositories;
using SMSSender.Messaging.Models;

namespace SMSSender.Messaging.Services
{
    public class FailedSmsLogger : IFailedSmsLogger
    {
        private readonly IUnitOfWork _unitOfWork;

        public FailedSmsLogger(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task LogAsync(SmsMessagePure model, Guid transactionId, string errorReason)
        {
            var createdAt = DateTime.UtcNow.EgyptNow();

            try
            {
                await _unitOfWork.Repository<SmsMessageLog>().AddAsync(new SmsMessageLog
                {
                    TransactionId = transactionId,
                    Message = model.Message ?? string.Empty,
                    ErrorMessage = errorReason,
                    MsgStatus = MsgStatus.Failure.ToString(),
                    Provider = model.ProviderStr ?? string.Empty,
                    ProviderName = model.DeviceName ?? string.Empty,
                    ProviderPhone = model.PhoneNumber ?? string.Empty,
                    CreatedAt = model.CreatedAt,
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
                var rootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                var dateFolder = createdAt.ToString("yyyy-MM-dd");
                var targetDirectory = Path.Combine(rootPath, "sms-failures", dateFolder);
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
