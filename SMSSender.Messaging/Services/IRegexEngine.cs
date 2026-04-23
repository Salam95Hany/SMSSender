namespace SMSSender.Messaging.Services
{
    public interface IRegexEngine
    {
        bool IsMatch(string input, IEnumerable<string>? patterns);
        bool TryExtract(string input, IEnumerable<string>? patterns, out string? value, string groupName = "value");
        bool TryExtractGroups(string input, IEnumerable<string>? patterns, out IReadOnlyDictionary<string, string> groups, params string[] groupNames);
    }
}
