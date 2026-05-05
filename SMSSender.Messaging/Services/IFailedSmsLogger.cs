namespace SMSSender.Messaging.Services
{
    public interface IFailedSmsLogger
    {
        Task LogAsync(SmsMessagePure model, string errorReason,bool IsCorrectionProcess = false);
    }
}
