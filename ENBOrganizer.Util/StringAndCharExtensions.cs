using System.Collections.Generic;

namespace ENBOrganizer.Util
{
    public static class StringAndCharExtensions
    {
        public static bool EqualsIgnoreCase(this string value, string comparisonString)
        {
            return value.Trim().ToUpper() == comparisonString.Trim().ToUpper();
        }

        public static bool IsInvalidPathOrFileCharacter(this char value)
        {
            var invalidChars = new List<char> { '\\', '/', ':', '*', '?', '"', '<', '>', '|' };

            return invalidChars.Contains(value);
        }
    }
}