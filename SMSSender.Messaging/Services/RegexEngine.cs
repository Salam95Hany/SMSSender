using System.Collections.Concurrent;
using System.Text.RegularExpressions;

namespace SMSSender.Messaging.Services
{
    public class RegexEngine : IRegexEngine
    {
        private static readonly RegexOptions EngineOptions = RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase;
        private static readonly TimeSpan RegexTimeout = TimeSpan.FromSeconds(2);
        private readonly ConcurrentDictionary<string, Regex?> _cache = new(StringComparer.Ordinal);

        public bool IsMatch(string input, IEnumerable<string>? patterns)
        {
            if (string.IsNullOrWhiteSpace(input) || patterns is null)
            {
                return false;
            }

            foreach (var pattern in patterns)
            {
                var regex = GetRegex(pattern);
                if (regex is null)
                {
                    continue;
                }

                try
                {
                    if (regex.IsMatch(input))
                    {
                        return true;
                    }
                }
                catch (RegexMatchTimeoutException)
                {
                }
            }

            return false;
        }

        public bool TryExtract(string input, IEnumerable<string>? patterns, out string? value, string groupName = "value")
        {
            value = null;
            if (!TryExtractGroups(input, patterns, out var groups, groupName))
            {
                return false;
            }

            if (!groups.TryGetValue(groupName, out value) || string.IsNullOrWhiteSpace(value))
            {
                value = null;
                return false;
            }

            return true;
        }

        public bool TryExtractGroups(string input, IEnumerable<string>? patterns, out IReadOnlyDictionary<string, string> groups, params string[] groupNames)
        {
            groups = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            if (string.IsNullOrWhiteSpace(input) || patterns is null)
            {
                return false;
            }

            var requiredGroups = groupNames
                .Where(group => !string.IsNullOrWhiteSpace(group))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToArray();

            foreach (var pattern in patterns)
            {
                var regex = GetRegex(pattern);
                if (regex is null)
                {
                    continue;
                }

                Match match;
                try
                {
                    match = regex.Match(input);
                }
                catch (RegexMatchTimeoutException)
                {
                    continue;
                }

                if (!match.Success)
                {
                    continue;
                }

                var values = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                foreach (var groupName in requiredGroups)
                {
                    var group = match.Groups[groupName];
                    if (group.Success && !string.IsNullOrWhiteSpace(group.Value))
                    {
                        values[groupName] = group.Value.Trim();
                    }
                }

                if (values.Count == 0)
                {
                    continue;
                }

                groups = values;
                return true;
            }

            return false;
        }

        private Regex? GetRegex(string? pattern)
        {
            if (string.IsNullOrWhiteSpace(pattern))
            {
                return null;
            }

            return _cache.GetOrAdd(pattern, key =>
            {
                try
                {
                    return new Regex(key, EngineOptions, RegexTimeout);
                }
                catch (ArgumentException)
                {
                    return null;
                }
            });
        }
    }
}
