namespace SMSSender.Messaging.Services
{
    public interface IFailedSmsLogger
    {
        Task LogAsync(SmsMessagePure model, Guid TransactionId, string errorReason);
    }
}
