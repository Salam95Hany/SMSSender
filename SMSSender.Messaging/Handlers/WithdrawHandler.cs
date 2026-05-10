using SMSSender.Entities.Common;
using SMSSender.Entities.Models.Messaging;
using SMSSender.Interfaces.Repositories;
using SMSSender.Messaging.Services;

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
                await UpdateWithdrawalLimitsAsync(message.Amount, message.BalanceAfter, message.ProviderPhone);
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
            var Now = DateTime.UtcNow.EgyptNow();

            var Entity = await _unitOfWork.Repository<WalletDetail>().GetByIdAsync(w => w.PhoneNumber == phoneNumber);

            if (Entity != null)
            {
                if (Entity.LastDailyResetDate.Date < Now.Date)
                {
                    Entity.UsedDailyWithdrawal = 0;
                    Entity.LastDailyResetDate = Now;
                }

                if (Entity.LastMonthlyResetDate.Month != Now.Month || Entity.LastMonthlyResetDate.Year != Now.Year)
                {
                    Entity.UsedMonthlyWithdrawal = 0;
                    Entity.LastMonthlyResetDate = Now;
                }

                Entity.Amount = balanceAfter.Value;
                Entity.UsedDailyWithdrawal += amount.Value;
                Entity.UsedMonthlyWithdrawal += amount.Value;
            }
        }
    }
}
