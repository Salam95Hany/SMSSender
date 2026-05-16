using SMSSender.Entities.Common;
using SMSSender.Entities.Models.Messaging;
using SMSSender.Interfaces;
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
        private readonly INotificationService _notificationService;
        public ChargeWalletHandler(IUnitOfWork unitOfWork, INotificationService notificationService)
        {
            _unitOfWork = unitOfWork;
            _notificationService = notificationService;
        }
        public OperationType OperationType => OperationType.ChargeWallet;

        public async Task Handle(MessageTransaction message)
        {
            try
            {
                string NotBody = $"تم شحن رصيد · المحفظة: {message.ProviderPhone}";
                var Entity = await _unitOfWork.Repository<WalletDetail>().GetByIdAsync(w => w.PhoneNumber == message.ProviderPhone);
                if (Entity != null)
                {
                    Entity.LastRechargeDate = message.OperationMsgDateTime.Value;
                    _notificationService.CreateNotification("شحن رصيد", NotBody, message.Provider, NotificationTypes.BalanceInquiry, NotificationReferenceTypes.MessageTransaction, message.TransactionId);
                    await _unitOfWork.CompleteAsync();
                }
            }
            catch
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
