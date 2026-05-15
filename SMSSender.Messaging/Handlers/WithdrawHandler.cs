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
            await using var transaction = await _unitOfWork.BeginTransactionAsync();
            try
            {
                string NotBody = $"تم سحب جنيه {message.Amount:N2} من {message.FromPhone} · المحفظة: {message.ProviderPhone}";
                 _unitOfWork.Repository<MessageTransaction>().Add(message);
                await UpdateWithdrawalLimitsAsync(message.Amount, message.BalanceAfter, message.ProviderPhone);
                _notificationService.CreateNotification("سحب مبلغ جديد", NotBody, message.Provider, NotificationTypes.Deposit, NotificationReferenceTypes.MessageTransaction, message.TransactionId);
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
            await UpdateWithdrawalLimitsAsync(message.Amount, message.BalanceAfter, message.ProviderPhone);
            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdateWithdrawalLimitsAsync(double? amount, double? balanceAfter, string phoneNumber)
        {
            try
            {
                var Now = DateTime.UtcNow.EgyptNow();

                string sql = @"
                UPDATE sms.WalletDetails
                SET
                    Amount = @p0,

                    UsedDailyWithdrawal =
                        CASE
                            WHEN CAST(LastDailyResetDate AS DATE) < CAST(@p1 AS DATE)
                                THEN @p2
                            ELSE UsedDailyWithdrawal + @p2
                        END,

                    LastDailyResetDate =
                        CASE
                            WHEN CAST(LastDailyResetDate AS DATE) < CAST(@p1 AS DATE)
                                THEN @p1
                            ELSE LastDailyResetDate
                        END,

                    UsedMonthlyWithdrawal =
                        CASE
                            WHEN MONTH(LastMonthlyResetDate) <> MONTH(@p1)
                                 OR YEAR(LastMonthlyResetDate) <> YEAR(@p1)
                                THEN @p2
                            ELSE UsedMonthlyWithdrawal + @p2
                        END,

                    LastMonthlyResetDate =
                        CASE
                            WHEN MONTH(LastMonthlyResetDate) <> MONTH(@p1)
                                 OR YEAR(LastMonthlyResetDate) <> YEAR(@p1)
                                THEN @p1
                            ELSE LastMonthlyResetDate
                        END

                WHERE PhoneNumber = @p3
            ";

                await _unitOfWork.ExecuteSqlAsync(sql, balanceAfter.Value, Now, amount.Value, phoneNumber);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
