using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace SMSSender.Hubs
{
    [Authorize]
    public class NotificationHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            try
            {
                var customerId = Context.User?.FindFirst("CustomerId")?.Value;
                var branchId = Context.User?.FindFirst("BranchId")?.Value;
                var isAdmin = Context.User?.IsInRole("Admin") == true || Context.User?.IsInRole("Manager") == true;

                if (isAdmin)
                    await Groups.AddToGroupAsync(Context.ConnectionId, $"Customer_{customerId}");
                else
                    await Groups.AddToGroupAsync(Context.ConnectionId, $"Customer_{customerId}_Branch_{branchId}");

                await base.OnConnectedAsync();
            }
            catch (Exception ex)
            {
                throw;
            } 
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var customerId = Context.User?.FindFirst("CustomerId")?.Value;
            var branchId = Context.User?.FindFirst("BranchId")?.Value;
            var isAdmin = Context.User?.IsInRole("Admin") == true || Context.User?.IsInRole("Manager") == true;

            if (isAdmin)
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"Customer_{customerId}");
            else
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"Customer_{customerId}_Branch_{branchId}");

            await base.OnDisconnectedAsync(exception);
        }
    }
}
