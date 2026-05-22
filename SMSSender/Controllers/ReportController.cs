using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SMSSender.Entities.Common;
using SMSSender.Interfaces;
using System.Data;

namespace SMSSender.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpPost("GetWalletsReportSummaryByOperationType")]
        public async Task<ApiResponseModel<DataTable>> GetWalletsReportSummaryByOperationType(PagingFilterModel PagingFilter)
        {
            var result = await _reportService.GetWalletsReportSummaryByOperationType(PagingFilter);
            return result;
        }

        [HttpPost("GetWalletsReportByOperationType")]
        public async Task<ApiResponseModel<DataTable>> GetWalletsReportByOperationType(PagingFilterModel PagingFilter)
        {
            var result = await _reportService.GetWalletsReportByOperationType(PagingFilter);
            return result;
        }

        [HttpPost("GetWalletProfitReportSummary")]
        public async Task<ApiResponseModel<DataTable>> GetWalletProfitReportSummary(PagingFilterModel PagingFilter)
        {
            var result = await _reportService.GetWalletProfitReportSummary(PagingFilter);
            return result;
        }

        [HttpPost("GetWalletProfitReport")]
        public async Task<ApiResponseModel<DataTable>> GetWalletProfitReport(PagingFilterModel PagingFilter)
        {
            var result = await _reportService.GetWalletProfitReport(PagingFilter);
            return result;
        }

    }
}
