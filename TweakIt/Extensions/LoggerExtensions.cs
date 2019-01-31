using System;
using System.Diagnostics;
using UnityModManagerNet;

namespace TweakIt
{
    public static class LoggerExtensions
    {
        [Conditional("DEBUG")]
        public static void Debug(this UnityModManager.ModEntry.ModLogger logger, string str) => logger.Log(str);

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
}