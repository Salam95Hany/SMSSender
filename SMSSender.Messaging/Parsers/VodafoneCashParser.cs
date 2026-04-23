using SMSSender.Messaging.Models;
using SMSSender.Messaging.Services;

namespace SMSSender.Messaging.Parsers
{
    public class VodafoneCashParser : RegexMessageParserBase
    {
        public VodafoneCashParser(IRegexEngine regexEngine, IMessageProviderRegistry providerRegistry)
            : base(regexEngine, providerRegistry)
        {
        }

        public override ProviderType Provider => ProviderType.VodafoneCash;
    }
}
