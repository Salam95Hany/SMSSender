using SMSSender.Entities.Models.Messaging;
using SMSSender.Interfaces.Repositories;
using SMSSender.Messaging.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSSender.Messaging.Repositories
{
    public class MessageLogRepository : IMessageLogRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public MessageLogRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task LogMsgStatus(SmsMessageLog log)
        {
            await _unitOfWork.Repository<SmsMessageLog>().AddAsync(log);
            await _unitOfWork.CompleteAsync();
        }
    }
}
