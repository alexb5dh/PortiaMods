﻿using Harmony12;
using JetBrains.Annotations;
using Pathea.HomeNs;

namespace TweakIt.Patches
{
    /// <summary>
    /// Removes pause after placing cooking ingredient.
    /// </summary>
    internal static class RemoveCookingStun
    {
        private static bool Enabled => Main.Enabled && Main.Settings.RemoveCookingStun;

        [HarmonyPatch(typeof(CookMachineCtr), "StopInput")]
        private static class CookMachineCtrStopInput
        {
            [UsedImplicitly]
            private static bool Prefix()
            {
                if (!Enabled) return true;

                return false;
            }
        }
    }
}