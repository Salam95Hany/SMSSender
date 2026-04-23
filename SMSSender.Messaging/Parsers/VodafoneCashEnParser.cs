using SMSSender.Messaging.Models;
using SMSSender.Messaging.Services;

namespace SMSSender.Messaging.Parsers
{
    public class VodafoneCashEnParser : RegexMessageParserBase
    {
        public VodafoneCashEnParser(IRegexEngine regexEngine, IMessageProviderRegistry providerRegistry)
            : base(regexEngine, providerRegistry)
        {
        }

        public override ProviderType Provider => ProviderType.VodafoneCashEnglish;
    }
}
