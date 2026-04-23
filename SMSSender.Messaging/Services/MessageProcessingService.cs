using SMSSender.Entities.Models.Messaging;
using SMSSender.Messaging.Handlers;
using SMSSender.Messaging.Models;
using SMSSender.Messaging.Parsers;
using SMSSender.Messaging.Repositories;

namespace SMSSender.Messaging.Services
{
    public class MessageProcessingService : IMessageProcessingService
    {
        private readonly IFailedSmsLogger _failedSmsLogger;
        private readonly IMessageLogRepository _logRepo;
        private readonly IEnumerable<IOperationHandler> _operationHandlers;
        private readonly IEnumerable<IMessageParser> _parsers;
        private readonly IMessageProviderRegistry _providerRegistry;

        public MessageProcessingService(
            IMessageProviderRegistry providerRegistry,
            IEnumerable<IMessageParser> parsers,
            IEnumerable<IOperationHandler> operationHandlers,
            IMessageLogRepository logRepo,
            IFailedSmsLogger failedSmsLogger)
        {
            _providerRegistry = providerRegistry;
            _parsers = parsers;
            _operationHandlers = operationHandlers;
            _logRepo = logRepo;
            _failedSmsLogger = failedSmsLogger;
        }

        public async Task<bool> Process(SmsMessagePure model)
        {
            var transactionId = Guid.NewGuid();
            var rawMessage = model.Message ?? string.Empty;

            try
            {
                if (!_providerRegistry.TryResolve(model.ProviderStr, rawMessage, out var providerDefinition))
                {
                    return await FailAsync(transactionId, model, "Provider could not be resolved.");
                }

                var parser = _parsers.FirstOrDefault(item => item.Provider == providerDefinition.ProviderType);
                if (parser is null)
                {
                    return await FailAsync(transactionId, model, $"Parser is not registered for provider '{providerDefinition.ProviderType}'.");
                }

                var parsedMessage = parser.Parse(model);
                if (!parsedMessage.OperationType.HasValue)
                {
                    return await FailAsync(transactionId, model, $"Operation type could not be detected for provider '{providerDefinition.ProviderType}'.");
                }

                var handler = _operationHandlers.FirstOrDefault(item => item.OperationType == parsedMessage.OperationType.Value);
                if (handler is null)
                {
                    return await FailAsync(transactionId, model, $"Operation handler is not registered for '{parsedMessage.OperationType.Value}'.");
                }

                var transaction = new MessageTransaction
                {
                    TransactionId = transactionId,
                    Provider = parsedMessage.Provider,
                    ProviderName = model.DeviceName,
                    ProviderPhone = model.PhoneNumber,
                    OperationType = parsedMessage.OperationType.Value,
                    Amount = parsedMessage.Amount.HasValue ? (double)parsedMessage.Amount.Value : null,
                    FromPhone = parsedMessage.FromPhone,
                    SenderName = parsedMessage.SenderName,
                    BalanceAfter = parsedMessage.BalanceAfter.HasValue ? (double)parsedMessage.BalanceAfter.Value : null,
                    TransactionNumber = parsedMessage.TransactionNumber,
                    OperationServerDateTime = DateTime.Now,
                    OperationMsgDateTime = parsedMessage.OperationDateTime,
                    OperationSentDateTime = parsedMessage.SentDateTime
                };

                await handler.Handle(transaction);
                await TryLogMessageStatusAsync(model, transactionId, MsgStatus.Success.ToString(), string.Empty);
                return true;
            }
            catch (Exception ex)
            {
                return await FailAsync(transactionId, model, ex.Message);
            }
        }

        private async Task<bool> FailAsync(Guid transactionId, SmsMessagePure model, string reason)
        {
            await _failedSmsLogger.LogAsync(model.Message ?? string.Empty, model.ProviderStr ?? string.Empty, reason);
            await TryLogMessageStatusAsync(model, transactionId, MsgStatus.Failure.ToString(), reason);
            return false;
        }

        private async Task TryLogMessageStatusAsync(SmsMessagePure model, Guid transactionId, string status, string errorMessage)
        {
            try
            {
                await _logRepo.LogMsgStatus(new SmsMessageLog
                {
                    TransactionId = transactionId,
                    Message = model.Message ?? string.Empty,
                    ErrorMessage = errorMessage,
                    MsgStatus = status,
                    Provider = model.ProviderStr ?? string.Empty,
                    ProviderName = model.DeviceName ?? string.Empty,
                    ProviderPhone = model.PhoneNumber ?? string.Empty,
                    SentStamp = model.SentStamp ?? string.Empty,
                    ReceivedStamp = model.ReceivedStamp ?? string.Empty,
                    Sim = model.Sim ?? string.Empty,
                    CreatedDate = DateTime.Now,
                });
            }
            catch
            {
            }
        }
    }
}
