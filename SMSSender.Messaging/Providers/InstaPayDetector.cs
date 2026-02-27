using SMSSender.Messaging.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSSender.Messaging.Providers
{
    public class InstaPayDetector : IMessageProviderDetector
    {
        public ProviderType ProviderType => ProviderType.InstaPay;

        public bool CanHandle(ProviderType Provider)
        {
            return Provider == ProviderType;
        }
    }
}
