using SMSSender.Messaging.Models;
using SMSSender.Messaging.Services;

namespace SMSSender.Messaging.Parsers
{
    public class InstaPayParser : RegexMessageParserBase
    {
        public InstaPayParser(IRegexEngine regexEngine, IMessageProviderRegistry providerRegistry)
            : base(regexEngine, providerRegistry)
        {
        }

        public override ProviderType Provider => ProviderType.InstaPay;
    }
}
