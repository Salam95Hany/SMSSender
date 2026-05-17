using SMSSender.Entities.Common;
using SMSSender.Entities.Models.Messaging;
using SMSSender.Interfaces;
using SMSSender.Interfaces.Repositories;
using SMSSender.Messaging.Services;

namespace SMSSender.Messaging.Handlers
{
    public class WithdrawHandler : IOperationHandler
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotificationService _notificationService;
        public WithdrawHandler(IUnitOfWork unitOfWork, INotificationService notificationService)
        {
            _unitOfWork = unitOfWork;
            _notificationService = notificationService;
        }
        public OperationType OperationType => OperationType.Withdraw;

        public async Task Handle(MessageTransaction message)
        {
            try
            {
                string NotBody = $"تم سحب {message.Amount:N2} جنيه من {message.FromPhone ?? message.SenderName} · المحفظة: {message.ProviderPhone}";
                 _unitOfWork.Repository<MessageTransaction>().Add(message);
                await UpdateWithdrawalLimitsAsync(message.Amount, message.BalanceAfter, message.ProviderPhone);
                _notificationService.CreateNotification("سحب مبلغ جديد", NotBody, message.Provider, NotificationTypes.Deposit, NotificationReferenceTypes.MessageTransaction, message.TransactionId);
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
            await UpdateWithdrawalLimitsAsync(message.Amount, message.BalanceAfter, message.ProviderPhone);
            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdateWithdrawalLimitsAsync(double? amount, double? balanceAfter, string phoneNumber)
        {
            var Now = DateTime.UtcNow.EgyptNow();

            var Entity = await _unitOfWork.Repository<WalletDetail>().GetByIdAsync(w => w.PhoneNumber == phoneNumber);

            if (Entity != null)
            {
                Entity.Amount = balanceAfter.HasValue ? balanceAfter.Value : 0;
                Entity.UsedDailyWithdrawal += amount.Value;
                Entity.UsedMonthlyWithdrawal += amount.Value;

                if (Entity.UsedDailyWithdrawal >= 55000)
                {
                    if (Entity.LastDailyWithdrawalLimitNotificationDate.Date < Now.Date)
                    {
                        _notificationService.CreateNotification(
                            "تنبيه الحد اليومي للسحب",
                            $"لقد اقتربت من الوصول للحد اليومي للسحب . المحفظة: {phoneNumber}",
                            null,
                            NotificationTypes.System,
                            NotificationReferenceTypes.WalletDetail
                        );

                        Entity.LastDailyWithdrawalLimitNotificationDate = Now;
                    }
                }

                if (Entity.UsedMonthlyWithdrawal >= 195000)
                {
                    if (Entity.LastMonthlyWithdrawalLimitNotificationDate.Month != Now.Month || Entity.LastMonthlyWithdrawalLimitNotificationDate.Year != Now.Year)
                    {
                        _notificationService.CreateNotification(
                            "تنبيه الحد الشهري للسحب",
                            $"لقد اقتربت من الوصول للحد الشهري للسحب . المحفظة: {phoneNumber}",
                            null,
                            NotificationTypes.System,
                            NotificationReferenceTypes.WalletDetail
                        );

                        Entity.LastMonthlyWithdrawalLimitNotificationDate = Now;
                    }
                }
            }
        }
    }
}
