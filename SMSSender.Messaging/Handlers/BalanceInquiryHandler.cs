using SMSSender.Entities.Common;
using SMSSender.Entities.Models.Messaging;
using SMSSender.Interfaces.Repositories;

namespace SMSSender.Messaging.Handlers
{
    public class BalanceInquiryHandler : IOperationHandler
    {
        private readonly IUnitOfWork _unitOfWork;

        public BalanceInquiryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public OperationType OperationType => OperationType.BalanceInquiry;

        public async Task Handle(MessageTransaction message)
        {
            await _unitOfWork.Repository<MessageTransaction>().AddAsync(message);
            await _unitOfWork.CompleteAsync();
        }
    }
}
