namespace SMSSender.Messaging.Services
{
    public interface IFailedSmsLogger
    {
        Task LogAsync(string rawMessage, string provider, string errorReason);
    }
}
