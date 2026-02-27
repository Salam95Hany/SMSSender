using SMSSender.Messaging.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSSender.Messaging.Repositories
{
    public class MessageLogRepository: IMessageLogRepository
    {
        private readonly List<MessageStatusLog> _logs = new();

        public void LogMsgStatus(MessageStatusLog log)
        {
            _logs.Add(log);
        }
    }
}
