using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMSSender.Entities.Common;
using SMSSender.Entities.Contracts.DTOs;
using SMSSender.Interfaces;
using SMSSender.Messaging;
using SMSSender.Messaging.Services;
using System.Data;

namespace SMSSender.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;
        private readonly IMessageProcessingService _processingService;
        public MessageController(IMessageService messageService, IMessageProcessingService processingService)
        {
            _messageService = messageService;
            _processingService = processingService;
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

        [HttpGet("GetMessageDetailsById")]
        public async Task<ApiResponseModel<MessageDetailsDto>> GetMessageDetailsById(Guid TransactionId)
        {
            var results = await _messageService.GetMessageDetailsById(TransactionId);
            return results;
        }

        [HttpPost("UpdateTransactionMessage")]
        public async Task<ApiResponseModel<MessageDetailsDto>> UpdateTransactionMessage(UpdateTransactionMessage Model)
        {
            var results = await _messageService.UpdateTransactionMessage(Model);
            return results;
        }

        [HttpPost("CorrectionProcess")]
        public async Task<bool> CorrectionProcess(SmsMessagePure Model)
        {
            var Process = await _processingService.CorrectionProcess(Model);
            return Process;
        }
    }
}
