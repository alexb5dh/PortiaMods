using System;
using System.Collections.Generic;
using System.Linq;
using Harmony12;
using InControl;
using Pathea.InputSolutionNs;

namespace Hotkeys.Patches
{
    /// <summary>
    /// Tweaks <see cref="PlayerActionModule"/> to remember <see cref="PlayerActionSet"/>s added before setup.
    /// </summary>
    [HarmonyPatch(typeof(PlayerActionModule), @"InitIncontrol")]
    internal static class FixEarlyInitializedInputsPatch
    {
        public static void Prefix(out List<PlayerActionSet> __state)
        {
            __state = null;

            try
            {
                var inputs = Traverse.Create(typeof(InputManager)).Field<List<PlayerActionSet>>("playerActionSets").Value;
                __state = inputs.ToList();
                inputs.Clear();

                Main.Logger.Debug($"[{nameof(FixEarlyInitializedInputsPatch)}] Saved {__state.Count} inputs");
            }
            catch (Exception exception) { Main.Logger.Exception(exception); }
        }

        public static void Postfix(List<PlayerActionSet> __state)
        {
            if (__state == null) return;

            try
            {
                var inputs = Traverse.Create(typeof(InputManager)).Field<List<PlayerActionSet>>("playerActionSets").Value;
                if (inputs.Any()) return;
                inputs.AddRange(__state);

                Main.Logger.Debug($"[{nameof(FixEarlyInitializedInputsPatch)}] Restored {__state?.Count} inputs");
            }
            catch (Exception exception) { Main.Logger.Exception(exception); }
        }
    }
}