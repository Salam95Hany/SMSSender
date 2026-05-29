using Microsoft.Data.SqlClient;
using SMSSender.Entities.Common;
using SMSSender.Entities.Models.Global;
using SMSSender.Entities.Models.Messaging;
using SMSSender.Interfaces;
using SMSSender.Interfaces.Common;
using SMSSender.Interfaces.Repositories;
using SMSSender.Services.Common;
using System.Data;

namespace SMSSender.Services
{
    public class WalletDetailService : IWalletDetailService
    {
        private readonly ISQLHelper _sQLHelper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentCustomerService _currentCustomerService;
        public WalletDetailService(ISQLHelper sQLHelper, IUnitOfWork unitOfWork, ICurrentCustomerService currentCustomerService)
        {
            _sQLHelper = sQLHelper;
            _unitOfWork = unitOfWork;
            _currentCustomerService = currentCustomerService;
        }

        public async Task<ApiResponseModel<DataTable>> GetWalletAccountSummary(PagingFilterModel PagingFilter)
        {
            var FromDate = PagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "DateRange")?.From;
            var ToDate = PagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "DateRange")?.To;
            var Params = new SqlParameter[4];
            Params[0] = new SqlParameter("@FromDate", FromDate);
            Params[1] = new SqlParameter("@ToDate", ToDate);
            Params[2] = new SqlParameter("@CustomerId", _currentCustomerService.CustomerId);
            Params[3] = new SqlParameter("@BranchId", _currentCustomerService.BranchId);
            var dt = await _sQLHelper.ExecuteDataTableAsync("[sms].[SP_GetWalletAccountSummary]", Params);
            return ApiResponseModel<DataTable>.Success(GenericErrors.GetSuccess, dt);
        }

        public async Task<ApiResponseModel<string>> UpdateInstaWalletAmount(int WalletDetailId, double Amount)
        {
            var Entity = await _unitOfWork.Repository<WalletDetail>().GetByIdAsync(WalletDetailId);
            if (Entity == null)
                return ApiResponseModel<string>.Failure(GenericErrors.TransFailed);

            Entity.Amount = Amount;

            await _unitOfWork.CompleteAsync();
            return ApiResponseModel<string>.Success(GenericErrors.UpdateSuccess);
        }
    }
}
