using SMSSender.Messaging.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSSender.Messaging.Providers
{
    public class VodafoneCashDetector : IMessageProviderDetector
    {
        public ProviderType ProviderType => ProviderType.VodafoneCash;

        public bool CanHandle(ProviderType Provider)
        {
            return Provider == ProviderType;
        }
    }
}
