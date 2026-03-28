using SMSSender.Entities.Models.Messaging;
using SMSSender.Messaging.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSSender.Messaging.Parsers
{
    public interface IProviderMessageParser
    {
        ProviderType Provider { get; }
        MessageTransaction Parse(string message);
    }
}
