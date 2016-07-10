namespace ENBOrganizer.Util
{
    public static class StringAndCharExtensions
    {
        public static bool EqualsIgnoreCase(this string value, string comparisonString)
        {
            return value?.Trim().ToUpper() == comparisonString?.Trim().ToUpper();
        }
    }
}