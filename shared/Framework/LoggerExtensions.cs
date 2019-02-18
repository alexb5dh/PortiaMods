using System;
using System.Diagnostics;
using UnityModManagerNet;

public static class LoggerExtensions
{
    public static void Log<T>(this UnityModManager.ModEntry.ModLogger logger, string str) => logger.Log($"[{typeof(T)}] {str}");

    [Conditional("DEBUG")]
    public static void Debug(this UnityModManager.ModEntry.ModLogger logger, string str) => logger.Log(str);

    [Conditional("DEBUG")]
    public static void Debug<T>(this UnityModManager.ModEntry.ModLogger logger, string str) => logger.Log<T>(str);

    [Conditional("DEBUG")]
    public static void StackTrace(this UnityModManager.ModEntry.ModLogger logger) => logger.Debug($"{new StackTrace()}");

    public static void Exception(this UnityModManager.ModEntry.ModLogger logger, Exception exc)
    {
#if DEBUG
        logger.Error($"{exc}");
#else
        logger.Error($"{exc.GetType()}: {exc.Message}");
#endif
    }
}