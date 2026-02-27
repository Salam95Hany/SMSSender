using SMSSender.Messaging.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSSender.Messaging.Handlers
{
    public class DepositHandler : IOperationHandler
    {
        public OperationType OperationType => OperationType.Deposit;

        public void Handle(ParsedOperationMessage message)
        {
            Console.WriteLine($"Deposit: {message.Amount} from {message.SenderName}");
            // هنا تضع منطق البيزنس
        }
    }
}
