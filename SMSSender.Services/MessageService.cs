using System.Text.RegularExpressions;
using SMSSender.Interfaces;
using SMSSender.Interfaces.Common;

namespace SMSSender.Services
{
    public class MessageService : IMessageService
    {
        private static readonly TimeSpan RegexTimeout = TimeSpan.FromSeconds(2);
        private readonly IAppSettings _appSettings;

        public MessageService(IAppSettings appSettings)
        {
            _appSettings = appSettings;
        }

        public bool GetMessageFiltered(string? provider, string message)
        {
            var normalizedMessage = SmsTextNormalizer.Normalize(message);
            if (string.IsNullOrWhiteSpace(normalizedMessage))
            {
                return false;
            }

            var normalizedProvider = SmsTextNormalizer.Normalize(provider);

            foreach (var providerSettings in _appSettings.MessageParsing.Providers.Values)
            {
                var senderMatched = providerSettings.SenderAliases.Any(alias =>
                    !string.IsNullOrWhiteSpace(alias) &&
                    normalizedProvider.Contains(alias, StringComparison.OrdinalIgnoreCase));

                var keywordMatched = providerSettings.OperationKeywords.All()
                    .Any(keyword => normalizedMessage.Contains(keyword, StringComparison.OrdinalIgnoreCase));

                var detectionMatched = providerSettings.DetectionPatterns.Any(pattern => SafeIsMatch(normalizedMessage, pattern));

                if ((senderMatched && (keywordMatched || detectionMatched)) || keywordMatched || detectionMatched)
                {
                    return true;
                }
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
