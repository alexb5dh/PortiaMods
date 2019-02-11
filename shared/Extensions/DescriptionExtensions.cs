using System;
using System.ComponentModel;
using System.Linq;

namespace Hotkeys.Extensions
{
    public static class DescriptionExtensions
    {
        public static string Description(this Type type) =>
            type.GetCustomAttributes(typeof(DescriptionAttribute), true)
                .Cast<DescriptionAttribute>()
                .FirstOrDefault()?.Description ?? type.Name;

        public static string Description<T>(this T instance) => instance.GetType().Description();
    }
}