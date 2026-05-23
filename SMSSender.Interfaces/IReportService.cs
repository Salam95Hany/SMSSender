using SMSSender.Entities.Common;
using SMSSender.Entities.Contracts.DTOs;
using SMSSender.Entities.Models.Messaging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSSender.Interfaces
{
    public interface IReportService
    {
        Task<ApiResponseModel<DataTable>> GetWalletsReportSummaryByOperationType(PagingFilterModel PagingFilter);
        Task<ApiResponseModel<DataTable>> GetWalletsReportByOperationType(PagingFilterModel PagingFilter);
        Task<ApiResponseModel<DataTable>> GetWalletProfitReportSummary(PagingFilterModel PagingFilter);
        Task<ApiResponseModel<DataTable>> GetWalletProfitReport(PagingFilterModel PagingFilter);
        Task<ApiResponseModel<List<ProfitClosingsDto>>> GetProfitPeriodClosings();
        Task<ApiResponseModel<ProfitClosingByDateDto>> GetProfitClosingByDate(DateTime FromDate, DateTime ToDate);
        Task<ApiResponseModel<string>> ProfitPeriodClosings(ProfitClosing Model);
    }
}
