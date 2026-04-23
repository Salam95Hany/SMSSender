using SMSSender.Interfaces.Common;
using SMSSender.Messaging.Models;

namespace SMSSender.Messaging.Services
{
    public class MessageProviderRegistry : IMessageProviderRegistry
    {
        private readonly IRegexEngine _regexEngine;
        private readonly IReadOnlyList<ProviderDefinition> _providers;
        private readonly IReadOnlyDictionary<ProviderType, ProviderDefinition> _providersByType;

        public MessageProviderRegistry(IAppSettings appSettings, IRegexEngine regexEngine)
        {
            _regexEngine = regexEngine;

            _providers = (appSettings.MessageParsing?.Providers ?? new Dictionary<string, ProviderSettings>())
                .Select(entry => TryCreateProvider(entry.Key, entry.Value))
                .Where(provider => provider is not null)
                .Cast<ProviderDefinition>()
                .OrderByDescending(provider => provider.Settings.Priority)
                .ToArray();

            _providersByType = _providers.ToDictionary(provider => provider.ProviderType, provider => provider);
        }

        public IReadOnlyCollection<ProviderDefinition> GetAll()
        {
            return _providers;
        }

        public bool TryGet(ProviderType providerType, out ProviderDefinition provider)
        {
            return _providersByType.TryGetValue(providerType, out provider!);
        }

        public bool TryResolve(string? providerSender, string message, out ProviderDefinition provider)
        {
            provider = null!;

            var normalizedSender = SmsTextNormalizer.Normalize(providerSender);
            var normalizedMessage = SmsTextNormalizer.Normalize(message);

            var candidates = _providers
                .Select(item => new
                {
                    Provider = item,
                    SenderMatched = MatchesSender(item, normalizedSender),
                    DetectionMatched = MatchesDetection(item, normalizedMessage)
                })
                .ToArray();

            provider = candidates.FirstOrDefault(candidate => candidate.SenderMatched && candidate.DetectionMatched)?.Provider
                ?? candidates.FirstOrDefault(candidate => candidate.DetectionMatched)?.Provider
                ?? candidates.FirstOrDefault(candidate => candidate.SenderMatched && candidate.Provider.Settings.DetectionPatterns.Length == 0)?.Provider
                ?? candidates.FirstOrDefault(candidate => candidate.SenderMatched)?.Provider
                ?? null!;

            return provider is not null;
        }

        private bool MatchesSender(ProviderDefinition provider, string sender)
        {
            if (string.IsNullOrWhiteSpace(sender))
            {
                return false;
            }

            return provider.Settings.SenderAliases.Any(alias =>
                !string.IsNullOrWhiteSpace(alias) &&
                sender.Contains(alias, StringComparison.OrdinalIgnoreCase));
        }

        private bool MatchesDetection(ProviderDefinition provider, string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return false;
            }

            return _regexEngine.IsMatch(message, provider.Settings.DetectionPatterns)
                || provider.Settings.OperationKeywords.All().Any(keyword => message.Contains(keyword, StringComparison.OrdinalIgnoreCase));
        }

        private static ProviderDefinition? TryCreateProvider(string key, ProviderSettings settings)
        {
            return Enum.TryParse<ProviderType>(key, ignoreCase: true, out var providerType)
                ? new ProviderDefinition(key, providerType, settings)
                : null;
        }
    }
}
