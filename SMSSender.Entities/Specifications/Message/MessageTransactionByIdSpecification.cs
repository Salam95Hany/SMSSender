using SMSSender.Entities.Models.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSSender.Entities.Specifications.Message
{
    public class MessageTransactionByIdSpecification:BaseSpecification<MessageTransaction>
    {
        public MessageTransactionByIdSpecification(Guid TransactionId):base(i => i.TransactionId == TransactionId)
        {
            
        }
    }

    public class MessageLogByIdSpecification : BaseSpecification<SmsMessageLog>
    {
        public MessageLogByIdSpecification(Guid TransactionId) : base(i => i.TransactionId == TransactionId)
        {

        }
    }
}
