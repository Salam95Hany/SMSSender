using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace SMSSender.Messaging.TaskQueue
{
    public class BackgroundTaskQueue : IBackgroundTaskQueue
    {
        private readonly Channel<SmsMessagePure> _queue;

        public BackgroundTaskQueue()
        {
            _queue = Channel.CreateUnbounded<SmsMessagePure>();
        }

        public async ValueTask QueueAsync(SmsMessagePure workItem)
        {
            await _queue.Writer.WriteAsync(workItem);
        }

        public async ValueTask QueueRangeAsync(IEnumerable<SmsMessagePure> workItems)
        {
            foreach (var item in workItems)
                await _queue.Writer.WriteAsync(item);
        }

        public async ValueTask<SmsMessagePure> DequeueAsync(CancellationToken cancellationToken)
        {
            return await _queue.Reader.ReadAsync(cancellationToken);
        }
    }
}
