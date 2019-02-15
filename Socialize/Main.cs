using System;
using System.Reflection;
using Harmony12;
using JetBrains.Annotations;
using UnityEngine;
using UnityModManagerNet;

namespace Socialize
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

            GUILayout.BeginVertical(OptionsGUI.Style);

            Settings.ShowFavor = GUILayout.Toggle(Settings.ShowFavor, new GUIContent(
                "Display favor value",
                "Display exact relationship value and value, required to progress in social tab."
            ));
            Settings.ShowGiftOptions = GUILayout.Toggle(Settings.ShowGiftOptions, new GUIContent(
                "Display gift options",
                "Display possible gifting options in item description."
            ));
            Settings.OrderGiftsByPreference = GUILayout.Toggle(Settings.OrderGiftsByPreference,new GUIContent(
                "Order gifts by preference", 
                "Order items by preference when selecting gift."
                ));
            Settings.ShowUnknownGiftOptions = GUILayout.Toggle(Settings.ShowUnknownGiftOptions, new GUIContent(
                "Consider undiscovered gifts",
                "Takes yet undiscovered gifting opportunities into account when ordering and providing item description."
            ));

            GUILayout.Space(20);
            GUILayout.Label(GUI.tooltip.IfNullOrEmpty("Hover over an option to provide more details."));

            GUILayout.EndVertical();
        }

        private static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            Enabled = value;
            return true;
        }

        private static void OnSaveGUI(UnityModManager.ModEntry modEntry) => Settings.Save(modEntry);
    }
}