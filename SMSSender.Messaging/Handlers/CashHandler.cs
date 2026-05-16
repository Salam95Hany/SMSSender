using SMSSender.Entities.Common;
using SMSSender.Entities.Models.Messaging;
using SMSSender.Interfaces;
using SMSSender.Interfaces.Repositories;
using SMSSender.Messaging.Services;

namespace SMSSender.Messaging.Handlers
{
    public class CashHandler : IOperationHandler
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotificationService _notificationService;
        public CashHandler(IUnitOfWork unitOfWork, INotificationService notificationService)
        {
            _unitOfWork = unitOfWork;
            _notificationService = notificationService;
        }
        public OperationType OperationType => OperationType.CashWithdrawal;

        public async Task Handle(MessageTransaction message)
        {
            try
            {
                string NotBody = $"تم سحب {message.Amount:N2} جنيه · المحفظة: {message.ProviderPhone}";
                _unitOfWork.Repository<MessageTransaction>().Add(message);
                _notificationService.CreateNotification("سحب نقدي", NotBody, message.Provider, NotificationTypes.BalanceInquiry, NotificationReferenceTypes.MessageTransaction, message.TransactionId);
                await _unitOfWork.CompleteAsync();
            }
            catch (Exception ex)
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
