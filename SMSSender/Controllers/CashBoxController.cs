using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMSSender.Entities.Common;
using SMSSender.Entities.Models.Messaging;
using SMSSender.Interfaces;
using System.Data;

namespace SMSSender.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CashBoxController : ControllerBase
    {
        private readonly ICashBoxService _cashBoxService;
        public CashBoxController(ICashBoxService cashBoxService)
        {
            _cashBoxService = cashBoxService;
        }

        [HttpPost("GetCashBoxData")]
        public async Task<ApiResponseModel<DataTable>> GetCashBoxData(PagingFilterModel PagingFilter)
        {
            var results = await _cashBoxService.GetCashBoxData(PagingFilter);
            return results;
        }

        [HttpPost("GetCashBoxFilters")]
        public async Task<ApiResponseModel<List<FilterModel>>> GetCashBoxFilters(PagingFilterModel PagingFilter)
        {
            var results = await _cashBoxService.GetCashBoxFilters(PagingFilter);
            return results;
        }

        [HttpPost("AddNewCashBox")]
        public async Task<ApiResponseModel<string>> AddNewCashBox(CashBox Model)
        {
            var results = await _cashBoxService.AddNewCashBox(Model);
            return results;
        }

        [HttpGet("DeleteCashBox")]
        public async Task<ApiResponseModel<string>> DeleteCashBox(int CashBoxId, string UserId)
        {
            var results = await _cashBoxService.DeleteCashBox(CashBoxId, UserId);
            return results;
        }
    }
}
