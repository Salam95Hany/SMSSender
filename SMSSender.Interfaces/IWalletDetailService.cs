using SMSSender.Entities.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSSender.Interfaces
{
    public interface IWalletDetailService
    {
        Task<ApiResponseModel<DataTable>> GetWalletAccountSummary(PagingFilterModel PagingFilter);
        Task<ApiResponseModel<string>> UpdateInstaWalletAmount(int WalletDetailId, double Amount);
    }
}
