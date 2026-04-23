using SMSSender.Messaging;
using SMSSender.Messaging.Models;

namespace SMSSender.Messaging.Parsers
{
    public interface IMessageParser
    {
        ProviderType Provider { get; }
        ParsedSmsMessage Parse(SmsMessagePure message);
    }
}
