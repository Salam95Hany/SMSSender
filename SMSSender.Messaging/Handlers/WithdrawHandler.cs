using SMSSender.Messaging.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSSender.Messaging.Handlers
{
    public class WithdrawHandler : IOperationHandler
    {
        public OperationType OperationType => OperationType.Withdraw;

        public void Handle(ParsedOperationMessage message)
        {
            Console.WriteLine($"Withdraw: {message.Amount} from {message.SenderName}");
            // هنا تضع منطق البيزنس
        }
    }
}
