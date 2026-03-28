using SMSSender.Messaging.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSSender.Messaging.Providers
{
    public class VodafoneCashEnDetector: IMessageProviderDetector
    {
        public ProviderType ProviderType => ProviderType.VodafoneCashEnglish;

        public bool CanHandle(ProviderType Provider)
        {
            return Provider == ProviderType;
        }
    }
}
