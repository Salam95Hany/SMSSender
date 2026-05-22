using SMSSender.Entities.Common;
using SMSSender.Entities.Models.Messaging;
using SMSSender.Interfaces;
using SMSSender.Interfaces.Repositories;
using SMSSender.Messaging.Models;
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
                await UpdateDepositLimitsAsync(message.Amount, message.BalanceAfter, message.ProviderPhone, message.Provider);
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
            await UpdateDepositLimitsAsync(message.Amount, message.BalanceAfter, message.ProviderPhone, message.Provider);
            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdateDepositLimitsAsync(double? amount, double? balanceAfter, string phoneNumber, string provider)
        {
            var Now = DateTime.UtcNow.EgyptNow();

            var Entity = await _unitOfWork.Repository<WalletDetail>().GetByIdAsync(w => w.PhoneNumber == phoneNumber && w.Type == provider);

            if (Entity != null)
            {
                if (provider == ProviderType.VodafoneCash.ToString())
                    Entity.Amount = balanceAfter.HasValue ? balanceAfter.Value : 0;
                else
                    Entity.Amount += amount.Value;
                Entity.UsedDailyDeposit += amount.Value;
                Entity.UsedMonthlyDeposit += amount.Value;

                if (Entity.UsedDailyDeposit >= 55000)
                {
                    if (Entity.LastDailyDepositLimitNotificationDate.Date < Now.Date)
                    {
                        _notificationService.CreateNotification(
                            "تنبيه الحد اليومي للإيداع",
                            $"لقد اقتربت من الوصول للحد اليومي للإيداع . المحفظة: {phoneNumber}",
                            null,
                            NotificationTypes.System,
                            NotificationReferenceTypes.WalletDetail
                        );

                        Entity.LastDailyDepositLimitNotificationDate = Now;
                    }
                }

                if (Entity.UsedMonthlyDeposit >= 195000)
                {
                    if (Entity.LastMonthlyDepositLimitNotificationDate.Month != Now.Month || Entity.LastMonthlyDepositLimitNotificationDate.Year != Now.Year)
                    {
                        _notificationService.CreateNotification(
                            "تنبيه الحد الشهري للإيداع",
                            $"لقد اقتربت من الوصول للحد الشهري للإيداع . المحفظة: {phoneNumber}",
                            null,
                            NotificationTypes.System,
                            NotificationReferenceTypes.WalletDetail
                        );

                        Entity.LastMonthlyDepositLimitNotificationDate = Now;
                    }
                }
            }
        }
    }
}
