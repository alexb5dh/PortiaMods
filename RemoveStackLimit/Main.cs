using System;
using System.Reflection;
using Harmony12;
using JetBrains.Annotations;
using RemoveStackLimit.Extensions;
using UnityEngine;
using UnityModManagerNet;

namespace RemoveStackLimit
{
    public static class Main
    {
        public static bool Enabled;
        public static Settings Settings;

        public static UnityModManager.ModEntry.ModLogger Logger { get; private set; }

        [UsedImplicitly]
        private static bool Load(UnityModManager.ModEntry modEntry)
        {
            try
            {
                Logger = modEntry.Logger;
                Settings = UnityModManager.ModSettings.Load<Settings>(modEntry);

                modEntry.OnToggle = OnToggle;
                modEntry.OnGUI = OnGUI;
                modEntry.OnSaveGUI = OnSaveGUI;

                var harmony = HarmonyInstance.Create($"com.{modEntry.Info.Author}.{modEntry.Info.Id}");
                harmony.PatchAll(Assembly.GetExecutingAssembly());

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
            if (Settings == null) return;

            GUILayout.BeginVertical(GetGUIStyle());

            GUILayout.BeginHorizontal();
            GUILayout.Label($"Stack size for items that can be stacked:", GUILayout.Width(400));
            Settings.StackSizeForStackable = Extensions.Int32.ParsePositiveOrNull(
                                                 GUILayout.TextField($"{Settings.StackSizeForStackable}", GUILayout.Width(100))
                                             ) ?? Settings.StackSizeForStackable;
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label($"Stack size for items that can't be stacked (ex. \"Practice Sword\"):", GUILayout.Width(400));
            Settings.StackSizeForUnstackable = Extensions.Int32.ParsePositiveOrNull(
                                                   GUILayout.TextField($"{Settings.StackSizeForUnstackable}", GUILayout.Width(100))
                                               ) ?? Settings.StackSizeForUnstackable;
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
        }

        private static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            Enabled = value;
            return true;
        }

        private static void OnSaveGUI(UnityModManager.ModEntry modEntry) => Settings.Save(modEntry);

        private static Texture2D GetBackground(Color color)
        {
            var background = new Texture2D(1, 1);
            background.SetPixel(0, 0, color);
            background.Apply();
            return background;
        }

        private static GUIStyle GetGUIStyle()
        {
            return new GUIStyle
            {
                normal = { background = GetBackground(new Color(0.35f, 0.35f, 0.35f, 1f)) },
                padding = { top = 5, bottom = 5, left = 5, right = 5 }
            };
        }
    }
}