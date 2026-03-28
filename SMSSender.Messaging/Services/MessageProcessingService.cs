using Microsoft.Extensions.Configuration;
using SMSSender.Entities.Common;
using SMSSender.Entities.Models.Messaging;
using SMSSender.Interfaces.Common;
using SMSSender.Messaging.Handlers;
using SMSSender.Messaging.Models;
using SMSSender.Messaging.Parsers;
using SMSSender.Messaging.Providers;
using SMSSender.Messaging.Repositories;
using System.Runtime.Serialization;

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

        public async Task<bool> Process(SmsMessagePure Model)
        {
            var TransactionId = Guid.NewGuid();
            try
            {
                if (Model.Message.Contains("Received") || Model.Message.Contains("received"))
                    Model.Provider = ProviderType.VodafoneCashEnglish;
                else
                    Model.Provider = ParseProvider(Model.ProviderStr);

                var detector = _providerDetectors.FirstOrDefault(d => d.CanHandle(Model.Provider));
                if (detector == null)
                    throw new Exception("Unknown Provider");

                var provider = detector.ProviderType;
                var operation = DetectOperation(Model.Message, provider.ToString());
                var parser = _parsers.First(p => p.Provider == provider);
                var parsedMessage = parser.Parse(Model.Message);
                parsedMessage.TransactionId = TransactionId;
                parsedMessage.OperationType = operation;
                parsedMessage.ProviderName = Model.DeviceName;
                parsedMessage.ProviderPhone = Model.PhoneNumber;
                var handler = _operationHandlers.First(h => h.OperationType == operation);
                await handler.Handle(parsedMessage);

                await _logRepo.LogMsgStatus(new SmsMessageLog
                {
                    TransactionId = TransactionId,
                    Message = Model.Message,
                    ErrorMessage = "",
                    MsgStatus = MsgStatus.Success.ToString(),
                    Provider = Model.ProviderStr,
                    ProviderName = Model.DeviceName,
                    ProviderPhone = Model.PhoneNumber,
                    SentStamp = Model.SentStamp,
                    ReceivedStamp = Model.ReceivedStamp,
                    Sim = Model.Sim,
                    CreatedDate = DateTime.Now,
                });

                return true;
            }
            catch (Exception ex)
            {
                await _logRepo.LogMsgStatus(new SmsMessageLog
                {
                    TransactionId = TransactionId,
                    Message = Model.Message,
                    ErrorMessage = ex.Message,
                    MsgStatus = MsgStatus.Failure.ToString(),
                    Provider = Model.ProviderStr,
                    ProviderName = Model.DeviceName,
                    ProviderPhone = Model.PhoneNumber,
                    SentStamp = Model.SentStamp,
                    ReceivedStamp = Model.ReceivedStamp,
                    Sim = Model.Sim,
                    CreatedDate = DateTime.Now,
                });

                return false;
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
            throw new Exception("Unknown Operation Type");
        }

        public ProviderType ParseProvider(string value)
        {
            foreach (var field in typeof(ProviderType).GetFields())
            {
                var attr = Attribute.GetCustomAttribute(field, typeof(EnumMemberAttribute)) as EnumMemberAttribute;

                if (attr != null && attr.Value == value)
                    return (ProviderType)field.GetValue(null);
            }

            throw new Exception("Unknown Provider");
        }
    }
}
