using Microsoft.AspNetCore.SignalR;
using SMSSender.Interfaces.Hub;

namespace SMSSender.Hubs
{
    public class HubNotificationService : IHubNotificationService
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        public HubNotificationService(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task SendMessageAddedAsync(int OperationType)
        {
            await _hubContext.Clients.All.SendAsync("Message_Added", OperationType);
        }

        public async Task SendMessageCalculatedAsync(int MessageTransactionId)
        {
            await _hubContext.Clients.All.SendAsync("Message_Calculated", MessageTransactionId);
        }
    }
}
