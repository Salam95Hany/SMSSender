using SMSSender.Entities.Common;
using SMSSender.Entities.Models.Messaging;
using SMSSender.Interfaces.Repositories;

namespace SMSSender.Messaging.Handlers
{
    public class TransferHandler : IOperationHandler
    {
        private readonly IUnitOfWork _unitOfWork;

        public TransferHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public OperationType OperationType => OperationType.Transfer;

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
    }
}
