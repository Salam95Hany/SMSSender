using SMSSender.Messaging.Models;

namespace SMSSender.Messaging.Services
{
    public interface IMessageProviderRegistry
    {
        IReadOnlyCollection<ProviderDefinition> GetAll();
        bool TryGet(ProviderType providerType, out ProviderDefinition provider);
        bool TryResolve(string? providerSender, string message, out ProviderDefinition provider);
    }
}
