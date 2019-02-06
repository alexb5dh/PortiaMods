namespace TweakIt
{
    public static class ColorExtensions
    {
        public static string Colored(this string @this, string color) => $"<color={color}>{@this}</color>";
    }
}