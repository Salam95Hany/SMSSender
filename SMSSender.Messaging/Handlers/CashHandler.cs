using SMSSender.Messaging.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSSender.Messaging.Handlers
{
    public class CashHandler : IOperationHandler
    {
        public OperationType OperationType => OperationType.Cash;

        public void Handle(ParsedOperationMessage message)
        {
            Console.WriteLine($"Cash: {message.Amount}");
            // هنا تضع منطق البيزنس
        }
    }
}
