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
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;
        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        [HttpPost("GetSmsDataByOperationType")]
        public async Task<ApiResponseModel<DataTable>> GetSmsDataByOperationType(PagingFilterModel PagingFilter)
        {
            var results = await _messageService.GetSmsDataByOperationType(PagingFilter);
            return results;
        }

        [HttpPost("GetSmsFilterByOperationType")]
        public async Task<ApiResponseModel<List<FilterModel>>> GetSmsFilterByOperationType(PagingFilterModel PagingFilter)
        {
            var results = await _messageService.GetSmsFilterByOperationType(PagingFilter);
            return results;
        }

        [HttpGet("GetOperationCountDashboardSummary")]
        public async Task<ApiResponseModel<DataTable>> GetOperationCountDashboardSummary()
        {
            var results = await _messageService.GetOperationCountDashboardSummary();
            return results;
        }

        [HttpGet("GetTodayLatestTransactions")]
        public async Task<ApiResponseModel<List<LatestTransactionDto>>> GetTodayLatestTransactions()
        {
            var results = await _messageService.GetTodayLatestTransactions();
            return results;
        }
    }
}
