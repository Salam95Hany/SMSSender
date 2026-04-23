using SMSSender.Entities.Common;
using SMSSender.Interfaces.Common;

namespace SMSSender.Messaging.Services
{
    public static class MessageParsingConfigService
    {
        public static IReadOnlyDictionary<OperationType, string[]> GetOperationKeywords(ProviderSettings providerSettings)
        {
            return new Dictionary<OperationType, string[]>
            {
                [OperationType.Deposit] = providerSettings.OperationKeywords.Deposit ?? Array.Empty<string>(),
                [OperationType.Withdraw] = providerSettings.OperationKeywords.Withdraw ?? Array.Empty<string>(),
                [OperationType.Transfer] = providerSettings.OperationKeywords.Transfer ?? Array.Empty<string>(),
                [OperationType.CashWithdrawal] = providerSettings.OperationKeywords.CashWithdrawal ?? Array.Empty<string>(),
                [OperationType.BalanceInquiry] = providerSettings.OperationKeywords.BalanceInquiry ?? Array.Empty<string>()
            };
        }

        public static IReadOnlyList<string> GetFieldPatterns(ProviderSettings providerSettings, string fieldName)
        {
            return providerSettings.FieldPatterns?.GetPatterns(fieldName) ?? Array.Empty<string>();
        }
    }
}
