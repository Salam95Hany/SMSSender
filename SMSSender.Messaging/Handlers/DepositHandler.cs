using SMSSender.Entities.Common;
using SMSSender.Entities.Models.Messaging;
using SMSSender.Interfaces;
using SMSSender.Interfaces.Repositories;
using SMSSender.Messaging.Services;

namespace SMSSender.Messaging.Handlers
{
    public class DepositHandler : IOperationHandler
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotificationService _notificationService;
        public DepositHandler(IUnitOfWork unitOfWork, INotificationService notificationService)
        {
            _unitOfWork = unitOfWork;
            _notificationService = notificationService;
        }
        public OperationType OperationType => OperationType.Deposit;

        public async Task Handle(MessageTransaction message)
        {
            try
            {
                string NotBody = $"تم إيداع {message.Amount:N2} جنيه من {message.FromPhone ?? message.SenderName} · المحفظة: {message.ProviderPhone}";
                _unitOfWork.Repository<MessageTransaction>().Add(message);
                await UpdateDepositLimitsAsync(message.Amount, message.BalanceAfter, message.ProviderPhone);
                _notificationService.CreateNotification("إيداع مبلغ جديد", NotBody, message.Provider, NotificationTypes.Deposit, NotificationReferenceTypes.MessageTransaction, message.TransactionId);
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
            await UpdateDepositLimitsAsync(message.Amount, message.BalanceAfter, message.ProviderPhone);
            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdateDepositLimitsAsync(double? amount, double? balanceAfter, string phoneNumber)
        {
            var Now = DateTime.UtcNow.EgyptNow();

            var Entity = await _unitOfWork.Repository<WalletDetail>().GetByIdAsync(w => w.PhoneNumber == phoneNumber);

            if (Entity != null)
            {
                if (Entity.LastDailyResetDate.Date < Now.Date)
                {
                    Entity.UsedDailyDeposit = 0;
                    Entity.LastDailyResetDate = Now;
                }

                if (Entity.LastMonthlyResetDate.Month != Now.Month || Entity.LastMonthlyResetDate.Year != Now.Year)
                {
                    Entity.UsedMonthlyDeposit = 0;
                    Entity.LastMonthlyResetDate = Now;
                }

                Entity.Amount = balanceAfter.Value;
                Entity.UsedDailyDeposit += amount.Value;
                Entity.UsedMonthlyDeposit += amount.Value;
            }
        }
    }
}
