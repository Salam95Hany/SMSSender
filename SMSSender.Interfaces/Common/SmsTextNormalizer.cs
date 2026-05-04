using System.Text;

namespace SMSSender.Interfaces.Common
{
    public static class SmsTextNormalizer
    {
        private static readonly HashSet<char> RemoveChars = new()
        {
            '\u200E', '\u200F',
            '\u202A', '\u202B', '\u202C', '\u202D', '\u202E',
            '\u2066', '\u2067', '\u2068', '\u2069',
            '\uFEFF',
            '\u200B', '\u200C', '\u200D',
            '\u180E',
            '\u00AD'
        };

        public static string Normalize(string? input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            input = input.Normalize(NormalizationForm.FormKC);

            var builder = new StringBuilder(input.Length);
            var previousWasWhitespace = false;

            foreach (var rawCharacter in input)
            {
                var character = NormalizeCharacter(rawCharacter);

                if (character == '\0')
                    continue;

                if (char.IsWhiteSpace(character))
                {
                    if (previousWasWhitespace)
                        continue;

                    builder.Append(' ');
                    previousWasWhitespace = true;
                    continue;
                }

                builder.Append(character);
                previousWasWhitespace = false;
            }

            return builder.ToString().Trim();
        }

        private static char NormalizeCharacter(char value)
        {
            if (IsIgnorableCharacter(value))
                return '\0';

            return value switch
            {
                '\u0660' => '0',
                '\u0661' => '1',
                '\u0662' => '2',
                '\u0663' => '3',
                '\u0664' => '4',
                '\u0665' => '5',
                '\u0666' => '6',
                '\u0667' => '7',
                '\u0668' => '8',
                '\u0669' => '9',
                '\u06F0' => '0',
                '\u06F1' => '1',
                '\u06F2' => '2',
                '\u06F3' => '3',
                '\u06F4' => '4',
                '\u06F5' => '5',
                '\u06F6' => '6',
                '\u06F7' => '7',
                '\u06F8' => '8',
                '\u06F9' => '9',
                '\u066B' => '.',
                '\u066C' => ',',
                '\u060C' => ',',
                '\u061B' => ';',
                '\u00A0' => ' ',

                _ => value
            };
        }

        private static bool IsIgnorableCharacter(char c)
        {
            if (RemoveChars.Contains(c))
                return true;

            return (c >= '\u2000' && c <= '\u200F') ||
                   (c >= '\u2028' && c <= '\u202F') ||
                   (c >= '\u2060' && c <= '\u206F');
        }
    }
}
