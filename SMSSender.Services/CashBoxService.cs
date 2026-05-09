using Microsoft.Data.SqlClient;
using SMSSender.Entities.Common;
using SMSSender.Entities.Models.Messaging;
using SMSSender.Interfaces;
using SMSSender.Interfaces.Common;
using SMSSender.Interfaces.Repositories;
using SMSSender.Services.Common;
using System.Data;

namespace SMSSender.Services
{
    public class CashBoxService : ICashBoxService
    {
        private readonly ISQLHelper _sQLHelper;
        private readonly IUnitOfWork _unitOfWork;
        public CashBoxService(ISQLHelper sQLHelper, IUnitOfWork unitOfWork)
        {
            _sQLHelper = sQLHelper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponseModel<DataTable>> GetCashBoxData(PagingFilterModel PagingFilter)
        {
            var FromDate = PagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "DateRange")?.From;
            var ToDate = PagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "DateRange")?.To;
            var FilterDt = PagingFilter.FilterList.ToDataTableFromFilterModel();
            var Params = new SqlParameter[6];
            Params[0] = new SqlParameter("@FilterList", FilterDt);
            Params[1] = new SqlParameter("@CurrentPage", PagingFilter.Currentpage);
            Params[2] = new SqlParameter("@PageSize", PagingFilter.Pagesize);
            Params[3] = new SqlParameter("@IsFilter", false);
            Params[4] = new SqlParameter("@FromDate", FromDate);
            Params[5] = new SqlParameter("@ToDate", ToDate);
            var dt = await _sQLHelper.ExecuteDataTableAsync("[sms].[SP_GetCashBoxData]", Params);
            return ApiResponseModel<DataTable>.Success(GenericErrors.GetSuccess, dt);
        }

        public async Task<ApiResponseModel<List<FilterModel>>> GetCashBoxFilters(PagingFilterModel PagingFilter)
        {
            var FromDate = PagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "DateRange")?.From;
            var ToDate = PagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "DateRange")?.To;
            var FilterDt = PagingFilter.FilterList.ToDataTableFromFilterModel();
            var Params = new SqlParameter[6];
            Params[0] = new SqlParameter("@FilterList", FilterDt);
            Params[1] = new SqlParameter("@CurrentPage", PagingFilter.Currentpage);
            Params[2] = new SqlParameter("@PageSize", PagingFilter.Pagesize);
            Params[3] = new SqlParameter("@IsFilter", true);
            Params[4] = new SqlParameter("@FromDate", FromDate);
            Params[5] = new SqlParameter("@ToDate", ToDate);
            var dt = await _sQLHelper.ExecuteDataTableAsync("[sms].[SP_GetCashBoxData]", Params);
            var Filters = dt.ToGroupedFilters();
            return ApiResponseModel<List<FilterModel>>.Success(GenericErrors.GetSuccess, Filters);
        }

        public async Task<ApiResponseModel<string>> AddNewCashBox(CashBox Model)
        {
            var LastTransaction = await _unitOfWork.Repository<CashBox>().GetLastAsync(i => i.InsertDate);
            double currentBalance = LastTransaction?.BalanceAfter ?? 0;
            double newBalance = 0;
            if (Model.MessageTransactionId.HasValue)
            {
                var MessageTrans = await _unitOfWork.Repository<MessageTransaction>().GetByIdAsync(Model.MessageTransactionId.Value);
                if (MessageTrans != null)
                {
                    MessageTrans.IsCalculated = true;
                    MessageTrans.Commission = (decimal)Model.Commission.Value;
                    MessageTrans.TransactionStatus = TransactionStatus.Completed;
                }

                if (Model.TransactionType == CashBoxTransactionType.Deposit)
                {
                    newBalance = currentBalance + Model.TransactionAmount.Value;
                }
                else
                {
                    if (Model.IsIncludeCommission.Value == false)
                    {
                        var Amount = Model.TransactionAmount - Model.Commission;
                        Model.TransactionAmount = Amount;
                        newBalance = currentBalance - Amount.Value;
                    }
                    else
                        newBalance = currentBalance - Model.TransactionAmount.Value;
                }
            }
            else
                newBalance = Model.TransactionType == CashBoxTransactionType.Deposit ? currentBalance + Model.TransactionAmount.Value : currentBalance - Model.TransactionAmount.Value;

            Model.Reason = GetCashBoxReason(Model.TransactionType, Model.MessageTransactionId, Model.ProviderPhone);
            Model.BalanceBefore = currentBalance;
            Model.BalanceAfter = newBalance;
            Model.InsertDate = DateTime.UtcNow.EgyptNow();

            await _unitOfWork.Repository<CashBox>().AddAsync(Model);
            await _unitOfWork.CompleteAsync();
            Model.CashBoxNumber = $"CB-{Model.CashBoxId:D6}";
            await _unitOfWork.CompleteAsync();

            return ApiResponseModel<string>.Success(GenericErrors.AddSuccess);
        }

        public async Task<ApiResponseModel<string>> DeleteCashBox(int CashBoxId, string UserId)
        {
            var Repo = _unitOfWork.Repository<CashBox>();

            var Entity = await Repo.GetByIdAsync(CashBoxId);

            if (Entity == null)
                return ApiResponseModel<string>.Failure(GenericErrors.NotFound);

            var lastTransaction = await _unitOfWork.Repository<CashBox>().GetLastAsync(i => i.InsertDate);
            double CurrentBalance = lastTransaction?.BalanceAfter ?? 0;

            Entity.IsDeleted = true;
            Entity.UpdateDate = DateTime.UtcNow.EgyptNow();
            await _unitOfWork.CompleteAsync();

            var ReversalAmount = Entity.TransactionAmount ?? 0;

            double BalanceBefore = CurrentBalance;
            double BalanceAfter;
            CashBoxTransactionType ReversalType;
            string ReasonAr;

            if (Entity.TransactionType == CashBoxTransactionType.Deposit)
            {
                ReversalType = CashBoxTransactionType.Withdraw;
                BalanceAfter = BalanceBefore - ReversalAmount;
                ReasonAr = $"تم إلغاء عملية إيداع بقيمة {ReversalAmount} جنيه";
            }
            else
            {
                ReversalType = CashBoxTransactionType.Deposit;
                BalanceAfter = BalanceBefore + ReversalAmount;
                ReasonAr = $"تم إلغاء عملية سحب بقيمة {ReversalAmount} جنيه";
            }

            var reversal = new CashBox
            {
                MessageTransactionId = Entity.MessageTransactionId,
                TransactionType = ReversalType,
                TransactionAmount = ReversalAmount,
                Reason = ReasonAr,
                BalanceBefore = BalanceBefore,
                BalanceAfter = BalanceAfter,
                InsertUser = UserId,
                IsDeleted = true,
                InsertDate = DateTime.UtcNow.EgyptNow(),
            };

            await Repo.AddAsync(reversal);
            await _unitOfWork.CompleteAsync();
            reversal.CashBoxNumber = $"CB-{reversal.CashBoxId:D6}";
            await _unitOfWork.CompleteAsync();
            return ApiResponseModel<string>.Success(GenericErrors.DeleteSuccess);
        }

        private string GetCashBoxReason(CashBoxTransactionType TransactionType, int? MessageTransactionId, string ProviderPhone)
        {
            if (!MessageTransactionId.HasValue)
            {
                if (TransactionType == CashBoxTransactionType.Deposit)
                    return "ايداع يدوي";
                else
                    return "سحب يدوي";
            }
            else
            {
                if (TransactionType == CashBoxTransactionType.Deposit)
                    return $"عملية ايداع من محفظة رقم ({ProviderPhone})";
                else
                    return $"عملية سحب من محفظة رقم ({ProviderPhone})";
            }
        }
    }
}
