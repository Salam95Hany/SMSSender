using SMSSender.Entities.Common;
using SMSSender.Entities.Contracts.DTOs;
using SMSSender.Entities.Models.Messaging;
using System.Data;

namespace SMSSender.Interfaces
{
    public interface IMessageService
    {
        Task<ApiResponseModel<DataTable>> GetSmsDataByOperationType(PagingFilterModel PagingFilter);
        Task<ApiResponseModel<List<FilterModel>>> GetSmsFilterByOperationType(PagingFilterModel PagingFilter);
        Task<ApiResponseModel<DataTable>> GetOperationCountDashboardSummary();
        Task<ApiResponseModel<List<LatestTransactionDto>>> GetTodayLatestTransactions();
        bool GetMessageFiltered(string? provider, string message);
        Task<ApiResponseModel<MessageDetailsDto>> GetMessageDetailsById(Guid TransactionId);
        Task<ApiResponseModel<MessageDetailsDto>> UpdateTransactionMessage(UpdateTransactionMessage Model);
    }
}
