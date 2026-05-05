using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSSender.Messaging.Services
{
    public interface IMessageProcessingService
    {
        Task<bool> Process(SmsMessagePure Message);
        Task<bool> CorrectionProcess(SmsMessagePure model);
    }
}
