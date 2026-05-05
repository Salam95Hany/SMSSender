using SMSSender.Entities.Common;
using SMSSender.Entities.Models.Messaging;
using SMSSender.Interfaces.Repositories;

namespace SMSSender.Messaging.Handlers
{
    public class DepositHandler : IOperationHandler
    {
        private readonly IUnitOfWork _unitOfWork;
        public DepositHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public OperationType OperationType => OperationType.Deposit;

        public async Task Handle(MessageTransaction message)
        {
            try
            {
                await _unitOfWork.Repository<MessageTransaction>().AddAsync(message);
                await _unitOfWork.CompleteAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task Update(MessageTransaction message)
        {
            _unitOfWork.Repository<MessageTransaction>().Update(message);
            await _unitOfWork.CompleteAsync();
        }
    }
}
