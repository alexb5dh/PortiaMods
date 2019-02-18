using UnityEngine;


public static class ColorExtensions
{
    public static string Colored(this string @this, string color) => $"<color={color}>{@this}</color>";

    public static string Colored(this string @this, Color color) => @this.Colored($"#{ColorUtility.ToHtmlStringRGBA(color)}");

    public static string Bold(this string @this) => $"<b>{@this}</b>";
}