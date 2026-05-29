using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SMSSender.Entities.Common;
using SMSSender.Entities.Contracts.DTOs;
using SMSSender.Entities.Models.Global;
using SMSSender.Entities.Models.Messaging;
using SMSSender.Entities.Specifications.Message;
using SMSSender.Interfaces;
using SMSSender.Interfaces.Common;
using SMSSender.Interfaces.Repositories;
using SMSSender.Services.Common;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;

namespace SMSSender.Services
{
    public class MessageService : IMessageService
    {
        private static readonly TimeSpan RegexTimeout = TimeSpan.FromSeconds(2);
        private readonly IAppSettings _appSettings;
        private readonly ISQLHelper _sQLHelper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentCustomerService _currentCustomerService;


        public MessageService(IAppSettings appSettings, ISQLHelper sQLHelper, IUnitOfWork unitOfWork, ICurrentCustomerService currentCustomerService)
        {
            _appSettings = appSettings;
            _sQLHelper = sQLHelper;
            _unitOfWork = unitOfWork;
            _currentCustomerService = currentCustomerService;
        }

        public async Task<ApiResponseModel<DataTable>> GetSmsDataByOperationType(PagingFilterModel PagingFilter)
        {
            var FromDate = PagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "DateRange")?.From;
            var ToDate = PagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "DateRange")?.To;
            var FilterDt = PagingFilter.FilterList.ToDataTableFromFilterModel();
            var Params = new SqlParameter[9];
            Params[0] = new SqlParameter("@FilterList", FilterDt);
            Params[1] = new SqlParameter("@CurrentPage", PagingFilter.Currentpage);
            Params[2] = new SqlParameter("@PageSize", PagingFilter.Pagesize);
            Params[3] = new SqlParameter("@IsFilter", false);
            Params[4] = new SqlParameter("@OperationType", PagingFilter.OperationType);
            Params[5] = new SqlParameter("@FromDate", FromDate);
            Params[6] = new SqlParameter("@ToDate", ToDate);
            Params[7] = new SqlParameter("@CustomerId", _currentCustomerService.CustomerId);
            Params[8] = new SqlParameter("@BranchId", _currentCustomerService.BranchId);
            var dt = await _sQLHelper.ExecuteDataTableAsync("[sms].[SP_GetSmsDateByOperationType]", Params);
            return ApiResponseModel<DataTable>.Success(GenericErrors.GetSuccess, dt);
        }

        public async Task<ApiResponseModel<List<FilterModel>>> GetSmsFilterByOperationType(PagingFilterModel PagingFilter)
        {
            var FromDate = PagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "DateRange")?.From;
            var ToDate = PagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "DateRange")?.To;
            var FilterDt = PagingFilter.FilterList.ToDataTableFromFilterModel();
            var Params = new SqlParameter[9];
            Params[0] = new SqlParameter("@FilterList", FilterDt);
            Params[1] = new SqlParameter("@CurrentPage", PagingFilter.Currentpage);
            Params[2] = new SqlParameter("@PageSize", PagingFilter.Pagesize);
            Params[3] = new SqlParameter("@IsFilter", true);
            Params[4] = new SqlParameter("@OperationType", PagingFilter.OperationType);
            Params[5] = new SqlParameter("@FromDate", FromDate);
            Params[6] = new SqlParameter("@ToDate", ToDate);
            Params[7] = new SqlParameter("@CustomerId", _currentCustomerService.CustomerId);
            Params[8] = new SqlParameter("@BranchId", _currentCustomerService.BranchId);
            var dt = await _sQLHelper.ExecuteDataTableAsync("[sms].[SP_GetSmsDateByOperationType]", Params);
            var Filters = dt.ToGroupedFilters();
            return ApiResponseModel<List<FilterModel>>.Success(GenericErrors.GetSuccess, Filters);
        }

        public async Task<ApiResponseModel<DataTable>> GetOperationCountDashboardSummary()
        {
            var Params = new SqlParameter[2];
            Params[0] = new SqlParameter("@CustomerId", _currentCustomerService.CustomerId);
            Params[1] = new SqlParameter("@BranchId", _currentCustomerService.BranchId);
            var dt = await _sQLHelper.ExecuteDataTableAsync("[sms].[SP_GetOperationCountDashboardSummary]", Params);
            return ApiResponseModel<DataTable>.Success(GenericErrors.GetSuccess, dt);
        }

        public async Task<ApiResponseModel<List<LatestTransactionDto>>> GetTodayLatestTransactions()
        {
            var Today = DateTime.UtcNow.EgyptNow().Date;
            var Tomorrow = Today.AddDays(1);

            var Data = await _unitOfWork.Repository<MessageTransaction>()
                .GetAllAsQueryable().Include(x => x.Branch).Where(x => x.OperationMsgDateTime >= Today && x.OperationMsgDateTime < Tomorrow)
                .OrderByDescending(x => x.OperationMsgDateTime)
                .Take(5).AsNoTracking().ToListAsync();

            var Results = Data.Select(x => new LatestTransactionDto
            {
                TransactionId = x.TransactionId,
                Provider = x.Provider == "InstaPay" ? "انستا باي" : "فودافون كاش",
                ProviderName = x.ProviderName,
                ProviderPhone = x.ProviderPhone,
                OperationTypeName =
                      x.OperationType == OperationType.Deposit ? "إيداع" :
                      x.OperationType == OperationType.Withdraw ? "سحب" :
                      x.OperationType == OperationType.CashWithdrawal ? "سحب نقدي" :
                      x.OperationType == OperationType.Transfer ? "إضافة غير نقدية" :
                      x.OperationType == OperationType.BalanceInquiry ? "استعلام رصيد" : "",
                OperationType = (int)x.OperationType,
                BranchName = x.Branch.Name,
                Amount = x.Amount,
                FromPhone = x.FromPhone,
                SenderName = x.SenderName,
                BalanceAfter = x.BalanceAfter,
                TransactionNumber = x.TransactionNumber,
                TransactionStatus = (int)x.TransactionStatus,
                OperationServerDateTime = x.OperationMsgDateTime.Value,
                Commission = x.Commission
            }).ToList();



            return ApiResponseModel<List<LatestTransactionDto>>.Success(GenericErrors.GetSuccess, Results);
        }

        public async Task<ApiResponseModel<DataTable>> GetMessageNotification(PagingFilterModel PagingFilter)
        {
            var Params = new SqlParameter[4];
            Params[0] = new SqlParameter("@CurrentPage", PagingFilter.Currentpage);
            Params[1] = new SqlParameter("@PageSize", PagingFilter.Pagesize);
            Params[2] = new SqlParameter("@CustomerId", _currentCustomerService.CustomerId);
            Params[3] = new SqlParameter("@BranchId", _currentCustomerService.BranchId);
            var dt = await _sQLHelper.ExecuteDataTableAsync("[sms].[SP_GetMessageNotification]", Params);
            return ApiResponseModel<DataTable>.Success(GenericErrors.GetSuccess, dt);
        }

        public async Task<ApiResponseModel<DataTable>> GetMessageBoxTodayData()
        {
            var Params = new SqlParameter[2];
            Params[0] = new SqlParameter("@CustomerId", _currentCustomerService.CustomerId);
            Params[1] = new SqlParameter("@BranchId", _currentCustomerService.BranchId);
            var dt = await _sQLHelper.ExecuteDataTableAsync("[sms].[SP_GetMessageBoxTodayData]", Params);
            return ApiResponseModel<DataTable>.Success(GenericErrors.GetSuccess, dt);
        }

        public async Task<ApiResponseModel<MessageDetailsDto>> GetMessageDetailsById(Guid TransactionId)
        {
            var LogSpec = new MessageLogByIdSpecification(TransactionId);
            var LogData = await _unitOfWork.Repository<SmsMessageLog>().GetByIdWithSpecAsync(LogSpec);
            var Results = new MessageDetailsDto
            {
                TransactionId = LogData.TransactionId,
                Provider = LogData.Provider,
                ProviderName = LogData.ProviderName,
                ProviderPhone = LogData.ProviderPhone,
                Sim = LogData.Sim,
                SentStamp = LogData.SentStamp,
                ReceivedStamp = LogData.ReceivedStamp,
                Message = LogData.Message
            };
            return ApiResponseModel<MessageDetailsDto>.Success(GenericErrors.GetSuccess, Results);
        }

        public async Task<ApiResponseModel<MessageDetailsDto>> UpdateTransactionMessage(UpdateTransactionMessage Model)
        {
            var Spec = new MessageTransactionByIdSpecification(Model.TransactionId);
            var Entity = await _unitOfWork.Repository<MessageTransaction>().GetByIdWithSpecAsync(Spec);
            if (Entity == null)
                return ApiResponseModel<MessageDetailsDto>.Failure(GenericErrors.TransFailed);

            Entity.Amount = Model.Amount;
            Entity.FromPhone = Model.FromPhone;
            Entity.SenderName = Model.SenderName;
            Entity.BalanceAfter = Model.BalanceAfter;
            Entity.Commission = Model.Commission.HasValue ? Model.Commission : null;

            await _unitOfWork.CompleteAsync();
            return ApiResponseModel<MessageDetailsDto>.Success(GenericErrors.UpdateSuccess);
        }

        public async Task<ApiResponseModel<string>> MakeMessageAsRead(int MessageTransactionId)
        {
            var Entity = await _unitOfWork.Repository<MessageTransaction>().GetByIdAsync(MessageTransactionId);
            if (Entity == null)
                return ApiResponseModel<string>.Failure(GenericErrors.TransFailed);

            //Entity.IsRead = true;

            await _unitOfWork.CompleteAsync();
            return ApiResponseModel<string>.Success(GenericErrors.UpdateSuccess);
        }

        public async Task<ApiResponseModel<string>> MakeMessageAsDelayed(int MessageTransactionId)
        {
            var Entity = await _unitOfWork.Repository<MessageTransaction>().GetByIdAsync(MessageTransactionId);
            if (Entity == null)
                return ApiResponseModel<string>.Failure(GenericErrors.TransFailed);

            Entity.IsCalculated = true;
            Entity.TransactionStatus = TransactionStatus.Delayed;

            await _unitOfWork.CompleteAsync();
            return ApiResponseModel<string>.Success(GenericErrors.UpdateSuccess);
        }

        public bool GetMessageFiltered(string? provider, string message)
        {
            var normalizedMessage = SmsTextNormalizer.Normalize(message);
            if (string.IsNullOrWhiteSpace(normalizedMessage))
                return false;

            var normalizedProvider = SmsTextNormalizer.Normalize(provider);

            foreach (var providerSettings in _appSettings.MessageParsing.Providers.Values)
            {
                var senderMatched = providerSettings.SenderAliases.Any(alias => !string.IsNullOrWhiteSpace(alias) && normalizedProvider.Contains(SmsTextNormalizer.Normalize(alias), StringComparison.OrdinalIgnoreCase));
                if (!senderMatched)
                    continue;

                var keywordMatched = providerSettings.OperationKeywords.All().Any(keyword => normalizedMessage.Contains(SmsTextNormalizer.Normalize(keyword), StringComparison.OrdinalIgnoreCase));
                var detectionMatched = providerSettings.DetectionPatterns.Any(pattern => SafeIsMatch(normalizedMessage, pattern));
                var hasAmount = providerSettings.FieldPatterns.GetPatterns(nameof(FieldPatterns.Amount)).Any(pattern => SafeIsMatch(normalizedMessage, pattern));
                var hasTransaction = providerSettings.FieldPatterns.GetPatterns(nameof(FieldPatterns.TransactionNumber)).Any(pattern => SafeIsMatch(normalizedMessage, pattern));
                var isStatementMessage = normalizedMessage.Contains("عدد العمليات");
                var isFinancialMessage = !isStatementMessage && (keywordMatched || detectionMatched) && (hasAmount || hasTransaction);

                if (isFinancialMessage)
                    return true;
            }

            return false;
        }

        private static bool SafeIsMatch(string message, string pattern)
        {
            if (string.IsNullOrWhiteSpace(pattern))
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(message, pattern, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant, RegexTimeout);
            }
            catch (ArgumentException)
            {
                return false;
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
    }
}
