using SMSSender.Interfaces.Common;

namespace SMSSender.Messaging.Models
{
    public sealed class ProviderDefinition
    {
        public ProviderDefinition(string key, ProviderType providerType, ProviderSettings settings)
        {
            Key = key;
            ProviderType = providerType;
            Settings = settings;
        }

        public string Key { get; }
        public ProviderType ProviderType { get; }
        public ProviderSettings Settings { get; }
    }
}
