namespace RemoveStackLimit.Extensions
{
    public static class Int32
    {
        public static int? ParsePositiveOrNull(string s)
        {
            return int.TryParse(s, out var number) && number > 0
                ? number
                : (int?)null;
        }
    }
}