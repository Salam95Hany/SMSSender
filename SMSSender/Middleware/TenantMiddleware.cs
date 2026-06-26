using SMSSender.Entities.Models.Global;

namespace SMSSender.Middleware
{
    public class TenantMiddleware
    {
        private readonly RequestDelegate _next;

        public TenantMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ICurrentCustomerService current)
        {
            var customerIdClaim = context.User?.FindFirst("CustomerId")?.Value;
            var branchIdClaim = context.User?.FindFirst("BranchId")?.Value;

            if (!string.IsNullOrEmpty(customerIdClaim))
                current.CustomerId = Guid.Parse(customerIdClaim);

            if (!string.IsNullOrEmpty(branchIdClaim))
                current.BranchId = int.Parse(branchIdClaim);

            current.IsAdmin = context.User?.IsInRole("Admin") == true || context.User?.IsInRole("Manager") == true;

            await _next(context);
        }
    }
}
