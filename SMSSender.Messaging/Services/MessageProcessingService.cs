using Microsoft.Extensions.Configuration;
using SMSSender.Interfaces.Common;
using SMSSender.Messaging.Handlers;
using SMSSender.Messaging.Models;
using SMSSender.Messaging.Parsers;
using SMSSender.Messaging.Providers;
using SMSSender.Messaging.Repositories;

namespace SMSSender.Messaging.Services
{
    public class MessageProcessingService : IMessageProcessingService
    {
        private readonly IEnumerable<IMessageProviderDetector> _providerDetectors;
        private readonly IEnumerable<IProviderMessageParser> _parsers;
        private readonly IEnumerable<IOperationHandler> _operationHandlers;
        private readonly IMessageLogRepository _logRepo;
        private readonly IAppSettings _appSettings;

        public MessageProcessingService(IEnumerable<IMessageProviderDetector> providerDetectors, IEnumerable<IProviderMessageParser> parsers, IEnumerable<IOperationHandler> operationHandlers, IMessageLogRepository logRepo, IAppSettings appSettings)
        {
            _providerDetectors = providerDetectors;
            _parsers = parsers;
            _operationHandlers = operationHandlers;
            _logRepo = logRepo;
            _appSettings = appSettings;
        }

        public void Process(SmsMessagePure Model)
        {
            var TransactionId = Guid.NewGuid();
            try
            {
                var detector = _providerDetectors.FirstOrDefault(d => d.CanHandle(Model.Provider));
                if (detector == null)
                {
                    _logRepo.LogMsgStatus(new MessageStatusLog
                    {
                        TransactionId = Guid.NewGuid(),
                        RawMessage = Model.Message,
                        ErrorMessage = "Unknown Provider",
                        MsgStatus = MsgStatus.Failure.ToString(),
                        CreatedAt = DateTime.UtcNow
                    });

                    return;
                }
                var provider = detector.ProviderType;

                var operation = DetectOperation(Model.Message, provider.ToString());

                var parser = _parsers.First(p => p.Provider == provider);
                var parsedMessage = parser.Parse(Model.Message);
                parsedMessage.TransactionId = TransactionId;
                parsedMessage.OperationType = operation;
                parsedMessage.ProviderName = Model.DeviceName;
                parsedMessage.ProviderPhone = Model.PhoneNumber;
                var handler = _operationHandlers.First(h => h.OperationType == operation);
                handler.Handle(parsedMessage);
                _logRepo.LogMsgStatus(new MessageStatusLog
                {
                    TransactionId = TransactionId,
                    RawMessage = Model.Message,
                    ErrorMessage = "",
                    MsgStatus = MsgStatus.Success.ToString(),
                    CreatedAt = DateTime.Now
                });
            }
            catch (Exception ex)
            {
                _logRepo.LogMsgStatus(new MessageStatusLog
                {
                    TransactionId = TransactionId,
                    RawMessage = Model.Message,
                    ErrorMessage = ex.Message,
                    MsgStatus = MsgStatus.Failure.ToString(),
                    CreatedAt = DateTime.Now
                });
            }
        }

        private OperationType DetectOperation(string message, string provider)
        {
            foreach (OperationType op in Enum.GetValues(typeof(OperationType)))
            {
                var keywords = MessageParsingConfigService.GetOperationKeywords(_appSettings, provider, op);
                if (keywords.Any(k => message.Contains(k)))
                    return op;
            }
            throw new Exception("Unknown operation type");
        }
    }
}
