using SMSSender.Messaging.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSSender.Messaging.Handlers
{
    public interface IOperationHandler
    {
        OperationType OperationType { get; }
        void Handle(ParsedOperationMessage message);
    }
}
