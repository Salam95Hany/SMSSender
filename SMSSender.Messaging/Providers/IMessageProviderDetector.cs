using SMSSender.Messaging.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSSender.Messaging.Providers
{
    public interface IMessageProviderDetector
    {
        ProviderType ProviderType { get; }
        bool CanHandle(ProviderType Provider);
    }
}
