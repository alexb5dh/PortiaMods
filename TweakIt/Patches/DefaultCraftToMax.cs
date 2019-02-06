using System;
using Harmony12;
using Pathea.UISystemNs;

namespace TweakIt.Patches
{
    /// <summary>
    /// Sets crafting/nutrient/ingredients amount to maximum value by default for all machines except worktable.
    /// </summary>
    public static class DefaultCraftToMax
    {
        private static bool Enabled => Main.Enabled && Main.Settings.DefaultCraftToMax;

        private static AutomataMachineMenuCtr _currentMachine;

        [HarmonyPatch(typeof(AutomataMachineMenuCtr), nameof(AutomataMachineMenuCtr.StartAutomata))]
        private static class AutomataMachineMenuCtrStartAutomata
        {
            private static void Prefix(AutomataMachineMenuCtr __instance) => _currentMachine = __instance;

            private static void Postfix() => _currentMachine = null;
        }

        [HarmonyPatch(typeof(UIUtils), nameof(UIUtils.ShowNumberSelectMinMax),
            typeof(int), typeof(int), typeof(int), typeof(int), typeof(string), 
            typeof(Action<int>), typeof(Action), typeof(bool), typeof(int), typeof(string))]
        private  static class UIUtilsShowNumberSelectMinMax
        {
            public static void Prefix(int max, ref int cur)
            {
                try
                {
                    if (!Enabled) return;

                    if (_currentMachine?.IsWorktable() ?? false) return;
                    if (cur > 0) cur = max;
                }
                catch (Exception exception) { Main.Logger.Exception(exception); }
            }
        }
    }
}