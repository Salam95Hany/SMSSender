using SMSSender.Entities.Common;
using SMSSender.Entities.Models.Messaging;
using SMSSender.Entities.Specifications.Message;
using SMSSender.Interfaces.Repositories;
using SMSSender.Messaging.Services;

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
            await UpdateDepositLimitsAsync(message.BalanceAfter, message.ProviderPhone);
            await _unitOfWork.CompleteAsync();
        }

        public async Task Update(MessageTransaction message)
        {
            _unitOfWork.Repository<MessageTransaction>().Update(message);
            await UpdateDepositLimitsAsync(message.BalanceAfter, message.ProviderPhone);
            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdateDepositLimitsAsync(double? balanceAfter, string phoneNumber)
        {
            try
            {
                string sql = @"
            UPDATE sms.WalletDetail
            SET Amount = @p0
            WHERE PhoneNumber = @p1";

                await _unitOfWork.ExecuteSqlAsync(sql, balanceAfter.Value, phoneNumber);
            }
            catch (Exception ex)
            {
                throw;
            } 
        }
    }
}
