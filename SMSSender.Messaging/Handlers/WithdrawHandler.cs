using SMSSender.Entities.Common;
using SMSSender.Entities.Models.Messaging;
using SMSSender.Interfaces.Repositories;

namespace SMSSender.Messaging.Handlers
{
    public class WithdrawHandler : IOperationHandler
    {
        private readonly IUnitOfWork _unitOfWork;
        public WithdrawHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public OperationType OperationType => OperationType.Withdraw;

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
