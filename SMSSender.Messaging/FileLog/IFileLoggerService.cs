using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSSender.Messaging.FileLog
{
    public interface IFileLoggerService
    {
        Task LogMessageData(object model);
        Task LogError(Exception ex);
    }
}
