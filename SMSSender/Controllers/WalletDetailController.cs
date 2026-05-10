using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SMSSender.Entities.Common;
using SMSSender.Interfaces;
using System.Data;

namespace SMSSender.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletDetailController : ControllerBase
    {
        private readonly IWalletDetailService _walletDetailService;
        public WalletDetailController(IWalletDetailService walletDetailService)
        {
            _walletDetailService = walletDetailService;
        }

        [HttpPost("GetWalletAccountSummary")]
        public Task<ApiResponseModel<DataTable>> GetWalletAccountSummary(PagingFilterModel PagingFilter)
        {
            var results = _walletDetailService.GetWalletAccountSummary(PagingFilter);
            return results;
        }
    }
}
