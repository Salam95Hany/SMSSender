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
            var customerIdHeader = context.Request.Headers["CustomerId"].FirstOrDefault();

            if (!string.IsNullOrEmpty(customerIdHeader))
            {
                current.CustomerId = Guid.Parse(customerIdHeader);
            }
            else
            {
                var customerIdClaim = context.User?.FindFirst("CustomerId")?.Value;

                if (!string.IsNullOrEmpty(customerIdClaim))
                    current.CustomerId = Guid.Parse(customerIdClaim);
            }

            var branchIdHeader = context.Request.Headers["BranchId"].FirstOrDefault();

            if (!string.IsNullOrEmpty(branchIdHeader))
            {
                current.BranchId = int.Parse(branchIdHeader);
            }
            else
            {
                var branchIdClaim = context.User?.FindFirst("BranchId")?.Value;

                if (!string.IsNullOrEmpty(branchIdClaim))
                    current.BranchId = int.Parse(branchIdClaim);
            }

            current.IsAdmin = context.User?.IsInRole("Admin") == true || context.User?.IsInRole("Manager") == true;

            await _next(context);
        }
    }
}
