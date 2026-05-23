using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SMSSender.Entities.Common;
using SMSSender.Entities.Contracts.DTOs;
using SMSSender.Entities.Models.Messaging;
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

        [HttpGet("GetProfitPeriodClosings")]
        public async Task<ApiResponseModel<List<ProfitClosingsDto>>> GetProfitPeriodClosings()
        {
            var result = await _reportService.GetProfitPeriodClosings();
            return result;
        }

        [HttpPost("ProfitPeriodClosings")]
        public async Task<ApiResponseModel<string>> ProfitPeriodClosings(ProfitClosing Model)
        {
            var result = await _reportService.ProfitPeriodClosings(Model);
            return result;
        }

        [HttpGet("GetProfitClosingByDate")]
        public async Task<ApiResponseModel<ProfitClosingByDateDto>> GetProfitClosingByDate(DateTime FromDate, DateTime ToDate)
        {
            var result = await _reportService.GetProfitClosingByDate(FromDate, ToDate);
            return result;
        }
    }
}
