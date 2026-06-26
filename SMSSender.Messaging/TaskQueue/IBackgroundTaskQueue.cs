using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSSender.Messaging.TaskQueue
{
    public interface IBackgroundTaskQueue
    {
        ValueTask QueueAsync(SmsMessagePure workItem);
        ValueTask QueueRangeAsync(IEnumerable<SmsMessagePure> workItems);
        ValueTask<SmsMessagePure> DequeueAsync(CancellationToken cancellationToken);
    }
}
