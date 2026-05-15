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
            await using var transaction = await _unitOfWork.BeginTransactionAsync();
            try
            {
                string NotBody = $"تم إيداع جنيه {message.Amount:N2} من {message.FromPhone} · المحفظة: {message.ProviderPhone}";
                _unitOfWork.Repository<MessageTransaction>().Add(message);
                await UpdateDepositLimitsAsync(message.Amount, message.BalanceAfter, message.ProviderPhone);
                _notificationService.CreateNotification("إيداع مبلغ جديد", NotBody, message.Provider, NotificationTypes.Deposit, NotificationReferenceTypes.MessageTransaction, message.TransactionId);
                await _unitOfWork.CompleteAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
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

            string sql = @"
                UPDATE sms.WalletDetails
                SET
                    Amount = @p0,

                    UsedDailyDeposit =
                        CASE
                            WHEN CAST(LastDailyResetDate AS DATE) < CAST(@p1 AS DATE)
                                THEN @p2
                            ELSE UsedDailyDeposit + @p2
                        END,

                    LastDailyResetDate =
                        CASE
                            WHEN CAST(LastDailyResetDate AS DATE) < CAST(@p1 AS DATE)
                                THEN @p1
                            ELSE LastDailyResetDate
                        END,

                    UsedMonthlyDeposit =
                        CASE
                            WHEN MONTH(LastMonthlyResetDate) <> MONTH(@p1)
                                 OR YEAR(LastMonthlyResetDate) <> YEAR(@p1)
                                THEN @p2
                            ELSE UsedMonthlyDeposit + @p2
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
    }
}
