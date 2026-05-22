using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSSender.Interfaces.Hub
{
    public interface IHubNotificationService
    {
        Task SendMessageAddedAsync(int OperationType);
        Task SendMessageCalculatedAsync(int MessageTransactionId);
    }
}
