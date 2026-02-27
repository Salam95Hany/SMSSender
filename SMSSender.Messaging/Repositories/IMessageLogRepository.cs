using SMSSender.Messaging.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSSender.Messaging.Repositories
{
    public interface IMessageLogRepository
    {
        void LogMsgStatus(MessageStatusLog log);
    }
}
