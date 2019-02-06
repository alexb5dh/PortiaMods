using System;
using System.Reflection;
using Harmony12;
using JetBrains.Annotations;
using UnityEngine;
using UnityModManagerNet;

namespace TweakIt
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
            Settings.ShowFavor = GUILayout.Toggle(Settings.ShowFavor, "Show numerical favor value in social tab");
            Settings.ShowUnknownGiftOptions = GUILayout.Toggle(Settings.ShowUnknownGiftOptions, "Show undiscovered gifting opportunities in item description");
            Settings.OrderGiftsByPreference =  GUILayout.Toggle(Settings.OrderGiftsByPreference, "Order items be preference when selecting gift");
            Settings.DefaultCraftToMax = GUILayout.Toggle(Settings.DefaultCraftToMax, "Set default craft amount to max");
            Settings.RemoveCookingStun = GUILayout.Toggle(Settings.RemoveCookingStun, "Disable pause after adding cooking ingredient");
            GUILayout.EndVertical();
        }

        private static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            Enabled = value;
            return true;
        }

        private static void OnSaveGUI(UnityModManager.ModEntry modEntry) => Settings.Save(modEntry);

        private static GUIStyle GetGUIStyle()
        {
            var background = new Texture2D(1, 1);
            background.SetPixel(0, 0, new Color(0.35f, 0.35f, 0.35f, 1f));
            background.Apply();

            return new GUIStyle { normal = { background = background }, padding = { top = 5, bottom = 5, left = 5, right = 5 } };
        }
    }
}