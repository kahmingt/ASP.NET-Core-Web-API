namespace WebApi.Shared.Extension
{
    public static class Extension
    {
        public static string TrimEnd(this string input, string suffixToRemove, StringComparison comparisonType = StringComparison.CurrentCulture)
        {
            input = input.Trim();

            if (suffixToRemove != null && input.EndsWith(suffixToRemove, comparisonType))
            {
                return input.Substring(0, input.Length - suffixToRemove.Length).Trim();
            }

            return input;
        }
    }
}
