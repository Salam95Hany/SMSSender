using Microsoft.Extensions.Configuration;
using SMSSender.Entities.Common;
using SMSSender.Interfaces.Common;
using SMSSender.Messaging.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSSender.Messaging.Services
{
    public static class MessageParsingConfigService
    {
        public static string[] GetOperationKeywords(IAppSettings appSettings, string provider, OperationType operation)
        {
            if (appSettings?.MessageParsing?.Providers != null
                && appSettings.MessageParsing.Providers.TryGetValue(provider, out var providerSettings)
                && providerSettings.OperationKeywords != null)
            {
                return operation switch
                {
                    OperationType.Deposit => providerSettings.OperationKeywords.Deposit ?? Array.Empty<string>(),
                    OperationType.Withdraw => providerSettings.OperationKeywords.Withdraw ?? Array.Empty<string>(),
                    OperationType.Cash => providerSettings.OperationKeywords.Cash ?? Array.Empty<string>(),
                    _ => Array.Empty<string>()
                };
            }

            return Array.Empty<string>();
        }

        public static string? GetFieldPattern(IAppSettings appSettings, string provider, string fieldName)
        {
            if (appSettings?.MessageParsing?.Providers != null
                && appSettings.MessageParsing.Providers.TryGetValue(provider, out var providerSettings)
                && providerSettings.FieldPatterns != null)
            {
                return fieldName switch
                {
                    "Amount" => providerSettings.FieldPatterns.Amount,
                    "FromPhone" => providerSettings.FieldPatterns.FromPhone,
                    "SenderName" => providerSettings.FieldPatterns.SenderName,
                    "BalanceAfter" => providerSettings.FieldPatterns.BalanceAfter,
                    "TransactionNumber" => providerSettings.FieldPatterns.TransactionNumber,
                    "OperationDateTime" => providerSettings.FieldPatterns.OperationDateTime,
                    _ => null
                };
            }

            return null;
        }
    }
}
