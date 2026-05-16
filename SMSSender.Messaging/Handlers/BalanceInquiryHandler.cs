using SMSSender.Entities.Common;
using SMSSender.Entities.Models.Messaging;
using SMSSender.Entities.Specifications.Message;
using SMSSender.Interfaces;
using SMSSender.Interfaces.Repositories;
using SMSSender.Messaging.Services;

namespace SMSSender.Messaging.Handlers
{
    public class BalanceInquiryHandler : IOperationHandler
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotificationService _notificationService;

        public BalanceInquiryHandler(IUnitOfWork unitOfWork, INotificationService notificationService)
        {
            _unitOfWork = unitOfWork;
            _notificationService = notificationService;
        }

        public OperationType OperationType => OperationType.BalanceInquiry;

        public async Task Handle(MessageTransaction message)
        {
            try
            {
                string NotBody = $"رصيدك الحالي {message.BalanceAfter:N2} جنيه · المحفظة: {message.ProviderPhone}";
                _unitOfWork.Repository<MessageTransaction>().Add(message);
                await UpdateDepositLimitsAsync(message.BalanceAfter, message.ProviderPhone);
                _notificationService.CreateNotification("استعلام رصيد", NotBody, message.Provider, NotificationTypes.BalanceInquiry, NotificationReferenceTypes.MessageTransaction, message.TransactionId);
                await _unitOfWork.CompleteAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task Update(MessageTransaction message)
        {
            _unitOfWork.Repository<MessageTransaction>().Update(message);
            await UpdateDepositLimitsAsync(message.BalanceAfter, message.ProviderPhone);
            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdateDepositLimitsAsync(double? balanceAfter, string phoneNumber)
        {
            var Entity = await _unitOfWork.Repository<WalletDetail>().GetByIdAsync(w => w.PhoneNumber == phoneNumber);
            if (Entity != null)
            {
                Entity.Amount = balanceAfter.Value;
            }
        }
    }
}
