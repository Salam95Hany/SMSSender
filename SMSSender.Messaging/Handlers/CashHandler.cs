using SMSSender.Entities.Common;
using SMSSender.Entities.Models.Messaging;
using SMSSender.Interfaces.Repositories;
using SMSSender.Messaging.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSSender.Messaging.Handlers
{
    public class CashHandler : IOperationHandler
    {
        private readonly IUnitOfWork _unitOfWork;
        public CashHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public OperationType OperationType => OperationType.CashWithdrawal;

        public async Task Handle(MessageTransaction message)
        {
            try
            {
                await _unitOfWork.Repository<MessageTransaction>().AddAsync(message);
                await _unitOfWork.CompleteAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
