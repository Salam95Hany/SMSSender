using SMSSender.Entities.Common;
using SMSSender.Entities.Models.Messaging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSSender.Interfaces
{
    public interface ICashBoxService
    {
        Task<ApiResponseModel<DataTable>> GetCashBoxData(PagingFilterModel PagingFilter);
        Task<ApiResponseModel<List<FilterModel>>> GetCashBoxFilters(PagingFilterModel PagingFilter);
        Task<ApiResponseModel<string>> AddNewCashBox(CashBox Model);
        Task<ApiResponseModel<string>> DeleteCashBox(int CashBoxId, string UserId);
    }
}
