using Microsoft.AspNetCore.SignalR;

namespace SMSSender.Hubs
{
    public class NotificationHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            var customerId = Context.User?.FindFirst("CustomerId")?.Value;
            var branchId = Context.User?.FindFirst("BranchId")?.Value;
            var isAdmin = Context.User?.IsInRole("Admin");

            if (isAdmin.GetValueOrDefault(false))
                await Groups.AddToGroupAsync(Context.ConnectionId, $"Customer_{customerId}");
            else
                await Groups.AddToGroupAsync(Context.ConnectionId, $"Customer_{customerId}_Branch_{branchId}");

            await base.OnConnectedAsync();
        }
    }
}
