using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SMSSender.Entities.Common;
using SMSSender.Entities.Contracts.DTOs;
using SMSSender.Entities.Models.Global;
using SMSSender.Entities.Models.Messaging;
using SMSSender.Interfaces;
using SMSSender.Interfaces.Common;
using SMSSender.Interfaces.Repositories;
using SMSSender.Services.Common;
using System.Data;

namespace SMSSender.Services
{
    public class ReportService : IReportService
    {
        private readonly ISQLHelper _sQLHelper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentCustomerService _currentCustomerService;
        public ReportService(ISQLHelper sQLHelper, IUnitOfWork unitOfWork, ICurrentCustomerService currentCustomerService)
        {
            _sQLHelper = sQLHelper;
            _unitOfWork = unitOfWork;
            _currentCustomerService = currentCustomerService;
        }

        public async Task<ApiResponseModel<DataTable>> GetWalletsReportSummaryByOperationType(PagingFilterModel PagingFilter)
        {
            var FromDate = PagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "DateRange")?.From;
            var ToDate = PagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "DateRange")?.To;
            var FilterDt = PagingFilter.FilterList.ToDataTableFromFilterModel();
            var Params = new SqlParameter[6];
            Params[0] = new SqlParameter("@FilterList", FilterDt);
            Params[1] = new SqlParameter("@OperationType", PagingFilter.OperationType);
            Params[2] = new SqlParameter("@FromDate", FromDate);
            Params[3] = new SqlParameter("@ToDate", ToDate);
            Params[4] = new SqlParameter("@CustomerId", _currentCustomerService.CustomerId);
            Params[5] = new SqlParameter("@BranchId", _currentCustomerService.IsAdmin ? 0 : _currentCustomerService.BranchId);
            var dt = await _sQLHelper.ExecuteDataTableAsync("[report].[SP_WalletsReportSummaryByOperationType]", Params);
            return ApiResponseModel<DataTable>.Success(GenericErrors.GetSuccess, dt);
        }

        public async Task<ApiResponseModel<DataTable>> GetWalletsReportByOperationType(PagingFilterModel PagingFilter)
        {
            var FromDate = PagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "DateRange")?.From;
            var ToDate = PagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "DateRange")?.To;
            var FilterDt = PagingFilter.FilterList.ToDataTableFromFilterModel();
            var Params = new SqlParameter[6];
            Params[0] = new SqlParameter("@FilterList", FilterDt);
            Params[1] = new SqlParameter("@OperationType", PagingFilter.OperationType);
            Params[2] = new SqlParameter("@FromDate", FromDate);
            Params[3] = new SqlParameter("@ToDate", ToDate);
            Params[4] = new SqlParameter("@CustomerId", _currentCustomerService.CustomerId);
            Params[5] = new SqlParameter("@BranchId", _currentCustomerService.IsAdmin ? 0 : _currentCustomerService.BranchId);
            var dt = await _sQLHelper.ExecuteDataTableAsync("[report].[SP_WalletsReportByOperationType]", Params);
            return ApiResponseModel<DataTable>.Success(GenericErrors.GetSuccess, dt);
        }

        public async Task<ApiResponseModel<DataTable>> GetWalletProfitReportSummary(PagingFilterModel PagingFilter)
        {
            var FromDate = PagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "DateRange")?.From;
            var ToDate = PagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "DateRange")?.To;
            var FilterDt = PagingFilter.FilterList.ToDataTableFromFilterModel();
            var Params = new SqlParameter[5];
            Params[0] = new SqlParameter("@FilterList", FilterDt);
            Params[1] = new SqlParameter("@FromDate", FromDate);
            Params[2] = new SqlParameter("@ToDate", ToDate);
            Params[3] = new SqlParameter("@CustomerId", _currentCustomerService.CustomerId);
            Params[4] = new SqlParameter("@BranchId", _currentCustomerService.IsAdmin ? 0 : _currentCustomerService.BranchId);
            var dt = await _sQLHelper.ExecuteDataTableAsync("[report].[SP_WalletProfitReportSummary]", Params);
            return ApiResponseModel<DataTable>.Success(GenericErrors.GetSuccess, dt);
        }

        public async Task<ApiResponseModel<DataTable>> GetWalletProfitReport(PagingFilterModel PagingFilter)
        {
            var FromDate = PagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "DateRange")?.From;
            var ToDate = PagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "DateRange")?.To;
            var FilterDt = PagingFilter.FilterList.ToDataTableFromFilterModel();
            var Params = new SqlParameter[5];
            Params[0] = new SqlParameter("@FilterList", FilterDt);
            Params[1] = new SqlParameter("@FromDate", FromDate);
            Params[2] = new SqlParameter("@ToDate", ToDate);
            Params[3] = new SqlParameter("@CustomerId", _currentCustomerService.CustomerId);
            Params[4] = new SqlParameter("@BranchId", _currentCustomerService.IsAdmin ? 0 : _currentCustomerService.BranchId);
            var dt = await _sQLHelper.ExecuteDataTableAsync("[report].[SP_WalletProfitReport]", Params);
            return ApiResponseModel<DataTable>.Success(GenericErrors.GetSuccess, dt);
        }

        public async Task<ApiResponseModel<string>> ProfitPeriodClosings(ProfitClosing Model)
        {
            var entity = await _unitOfWork.Repository<MessageTransaction>().AnyAsync(x => x.OperationMsgDateTime.Value.Date >= Model.FromDate.Date &&
               x.OperationMsgDateTime.Value.Date <= Model.ToDate.Date && x.Commission.HasValue && x.TransactionStatus == TransactionStatus.Delayed);
            if (entity)
                return ApiResponseModel<string>.Failure(GenericErrors.DelayedTransactionsExist);

            var Now = DateTime.UtcNow.EgyptNow();
            var CashBoxObj = new CashBox();
            var LastTransaction = await _unitOfWork.Repository<CashBox>().GetLastAsync(i => i.InsertDate);
            double currentBalance = LastTransaction?.BalanceAfter ?? 0;
            if (LastTransaction != null)
            {
                CashBoxObj.TransactionType = CashBoxTransactionType.ProfitClosingTransfer;
                CashBoxObj.Reason = $"سحب أرباح فترة ({Model.FromDate:dd-MM-yyyy} - {Model.ToDate:dd-MM-yyyy})";
                CashBoxObj.TransactionAmount = Model.NetProfit;
                CashBoxObj.BalanceBefore = LastTransaction.BalanceAfter;
                CashBoxObj.BalanceAfter = LastTransaction.BalanceAfter - Model.NetProfit;
                CashBoxObj.IsDeleted = true;
                CashBoxObj.InsertUser = Model.ClosedBy;
                CashBoxObj.InsertDate = Now;

                Model.CashBalanceBefore = LastTransaction.BalanceAfter;
                Model.CashBalanceAfter = LastTransaction.BalanceAfter - Model.NetProfit;
            }

            Model.ClosedDate = Now;

            await _unitOfWork.Repository<CashBox>().AddAsync(CashBoxObj);
            await _unitOfWork.Repository<ProfitClosing>().AddAsync(Model);
            await _unitOfWork.CompleteAsync();
            CashBoxObj.CashBoxNumber = $"CB-{CashBoxObj.CashBoxId:D6}";
            await _unitOfWork.CompleteAsync();

            return ApiResponseModel<string>.Success(GenericErrors.AddSuccess);
        }

        public async Task<ApiResponseModel<List<ProfitClosingsDto>>> GetProfitPeriodClosings()
        {
            var Results = await _unitOfWork.Repository<ProfitClosing>().GetAllAsQueryable().Select(i => new ProfitClosingsDto
            {
                From = i.FromDate,
                To = i.ToDate
            }).ToListAsync();

            return ApiResponseModel<List<ProfitClosingsDto>>.Success(GenericErrors.GetSuccess, Results);
        }

        public async Task<ApiResponseModel<ProfitClosingByDateDto>> GetProfitClosingByDate(DateTime FromDate, DateTime ToDate)
        {
            try
            {
                var totalProfit = await _unitOfWork.Repository<MessageTransaction>().SumAsync(
                x => x.OperationMsgDateTime.Value.Date >= FromDate.Date && x.OperationMsgDateTime.Value.Date <= ToDate.Date && x.Commission.HasValue
                && x.TransactionStatus == TransactionStatus.Completed,
                x => (double)x.Commission.Value);

                var netProfit = await _unitOfWork.Repository<MessageTransaction>()
                    .SumAsync(
                        x => x.OperationMsgDateTime.Value.Date >= FromDate.Date && x.OperationMsgDateTime.Value.Date <= ToDate.Date && x.Commission.HasValue
                        && x.TransactionStatus == TransactionStatus.Completed,
                        x =>
                            x.OperationType == OperationType.Deposit ? (double)x.Commission.Value :
                            x.OperationType == OperationType.Withdraw ? (double)x.Commission.Value :
                            x.OperationType == OperationType.CashWithdrawal ? -(double)x.Commission.Value :
                            0
                    );

                var Results = new ProfitClosingByDateDto
                {
                    TotalProfit = totalProfit,
                    NetProfit = netProfit
                };

                return ApiResponseModel<ProfitClosingByDateDto>.Success(GenericErrors.ProfitPeriodClosingSuccess, Results);
            }
            catch (Exception)
            {
                return ApiResponseModel<ProfitClosingByDateDto>.Failure(GenericErrors.TransFailed);
            }

        }
    }
}
