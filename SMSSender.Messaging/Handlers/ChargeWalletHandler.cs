using SMSSender.Entities.Common;
using SMSSender.Entities.Models.Messaging;
using SMSSender.Interfaces.Repositories;
using SMSSender.Messaging.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSSender.Messaging.Handlers
{
    public class ChargeWalletHandler : IOperationHandler
    {
        private readonly IUnitOfWork _unitOfWork;
        public ChargeWalletHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public OperationType OperationType => OperationType.ChargeWallet;

        public async Task Handle(MessageTransaction message)
        {
            try
            {
                var Entity = await _unitOfWork.Repository<WalletDetail>().GetByIdAsync(w => w.PhoneNumber == message.ProviderPhone);
                if (Entity != null)
                {
                    Entity.LastRechargeDate = message.OperationMsgDateTime;
                    await _unitOfWork.CompleteAsync();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Task Update(MessageTransaction message)
        {
            throw new NotImplementedException();
        }
    }
}
