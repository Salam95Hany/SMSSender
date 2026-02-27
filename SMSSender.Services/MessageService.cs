using SMSSender.Interfaces;
using SMSSender.Interfaces.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSSender.Services
{
    public class MessageService : IMessageService
    {
        private readonly IAppSettings _appSettings;
        public MessageService(IAppSettings appSettings)
        {
            _appSettings = appSettings;
        }

        public bool GetMessageFiltered(string Message)
        {
            if (string.IsNullOrWhiteSpace(Message)) return false;

            var allKeywords = _appSettings.MessageParsing.Providers.SelectMany(p => new[]
            {
                p.Value.OperationKeywords.Deposit,
                p.Value.OperationKeywords.Withdraw,
                p.Value.OperationKeywords.Cash
            }).SelectMany(x => x).Where(x => !string.IsNullOrWhiteSpace(x)).Distinct().ToList();

            return allKeywords.Any(keyword => Message.Contains(keyword, StringComparison.OrdinalIgnoreCase));
        }
    }
}
