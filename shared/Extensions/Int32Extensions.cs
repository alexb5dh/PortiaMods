public static class Int32Extensions
{
    public static int AbsMod(this int @this, int mod)
    {
        var result = @this % mod;
        return result < 0 ? result + mod : result;
    }
}