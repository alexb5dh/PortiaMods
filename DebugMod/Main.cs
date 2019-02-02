using System;
using DebugMod.Extensions;
using Harmony12;
using JetBrains.Annotations;
using UnityEngine;
using UnityModManagerNet;

namespace DebugMod
{
    public static class Main
    {
        private static HarmonyInstance _harmony;
        private static string _identifiers = "";

        public static bool Enabled;

        public static UnityModManager.ModEntry.ModLogger Logger { get; private set; }

        [UsedImplicitly]
        private static bool Load(UnityModManager.ModEntry modEntry)
        {
            try
            {
                Logger = modEntry.Logger;
                modEntry.OnGUI = OnGUI;

                _harmony = HarmonyInstance.Create($"com.{modEntry.Info.Author}.{modEntry.Info.Id}");

                return true;
            }
            catch (Exception exception)
            {
                Logger.Exception(exception);
                return false;
            }
        }

        private static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            GUILayout.BeginHorizontal();

            _identifiers = GUILayout.TextArea(_identifiers, GUILayout.Width(400), GUILayout.Height(400));

            if (GUILayout.Button("Patch", GUILayout.Width(100)))
            {
                try { Patch(_identifiers); }
                catch (Exception exception) { Logger.Exception(exception); }
            }

            GUILayout.EndVertical();
        }

        private static void StackTrace()
        {
            Logger.StackTrace();
        }

        private static void Patch(string values)
        {
            _harmony.UnpatchAllOwned();

            foreach (var fullName in values.Split(',', StringSplitOptions.RemoveEmptyEntries))
            {
                var method = AccessTools.Method(fullName);
                if (method == null)
                {
                    Logger.Debug($"{fullName} not found.");
                    continue;
                }

                _harmony.Patch(method, new HarmonyMethod(typeof(Main), nameof(StackTrace)));
            }
        }
    }
}