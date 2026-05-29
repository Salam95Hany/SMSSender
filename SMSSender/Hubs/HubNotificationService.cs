using Microsoft.AspNetCore.SignalR;
using SMSSender.Entities.Models.Global;
using SMSSender.Interfaces.Hub;

namespace SMSSender.Hubs
{
    public class HubNotificationService : IHubNotificationService
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly ICurrentCustomerService _currentCustomerService;
        public HubNotificationService(IHubContext<NotificationHub> hubContext, ICurrentCustomerService currentCustomerService)
        {
            _hubContext = hubContext;
            _currentCustomerService = currentCustomerService;
        }

        public async Task SendMessageAddedAsync(int OperationType, Guid CustomerId)
        {
            var customerId = _currentCustomerService.CustomerId;

            await _hubContext.Clients.Group($"Customer_{customerId}").SendAsync("Message_Added", OperationType);
        }

        public async Task SendMessageAddedAsync(int OperationType)
        {
            var customerId = _currentCustomerService.CustomerId;
            var branchId = _currentCustomerService.BranchId;

            await _hubContext.Clients.Group($"Customer_{customerId}_Branch_{branchId}").SendAsync("Message_Added", OperationType);
            await _hubContext.Clients.Group($"Customer_{customerId}").SendAsync("Message_Added", OperationType);
        }

        public async Task SendMessageCalculatedAsync(int MessageTransactionId)
        {
            var customerId = _currentCustomerService.CustomerId;
            var branchId = _currentCustomerService.BranchId;

            await _hubContext.Clients.Group($"Customer_{customerId}_Branch_{branchId}").SendAsync("Message_Calculated", MessageTransactionId);
            await _hubContext.Clients.Group($"Customer_{customerId}").SendAsync("Message_Calculated", MessageTransactionId);
        }
    }
}
