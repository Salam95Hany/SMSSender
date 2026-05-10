using Microsoft.Data.SqlClient;
using SMSSender.Entities.Common;
using SMSSender.Interfaces;
using SMSSender.Interfaces.Common;
using SMSSender.Interfaces.Repositories;
using SMSSender.Services.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSSender.Services
{
    public class WalletDetailService : IWalletDetailService
    {
        private readonly ISQLHelper _sQLHelper;
        private readonly IUnitOfWork _unitOfWork;
        public WalletDetailService(ISQLHelper sQLHelper, IUnitOfWork unitOfWork)
        {
            _sQLHelper = sQLHelper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponseModel<DataTable>> GetWalletAccountSummary(PagingFilterModel PagingFilter)
        {
            var FromDate = PagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "DateRange")?.From;
            var ToDate = PagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "DateRange")?.To;
            var Params = new SqlParameter[2];
            Params[0] = new SqlParameter("@FromDate", FromDate);
            Params[1] = new SqlParameter("@ToDate", ToDate);
            var dt = await _sQLHelper.ExecuteDataTableAsync("[sms].[SP_GetWalletAccountSummary]", Params);
            return ApiResponseModel<DataTable>.Success(GenericErrors.GetSuccess, dt);
        }
    }
}
