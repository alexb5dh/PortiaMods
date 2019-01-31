using System;
using Harmony12;
using Pathea.ItemSystem;
using RemoveStackLimit.Extensions;

namespace RemoveStackLimit.Patches
{
    /// <summary>
    /// Patches to remove stack limit.
    /// </summary>
    public static class RemoveStackLimitPatches
    {
        private static bool Enabled => Main.Enabled;

        private static void RemoveStackLimit(ItemBaseConfData data)
        {
            if (data == null || data.StackLimitedNumber == int.MaxValue) return;

            Main.Logger.Debug($"Removing stack limit of {data.StackLimitedNumber} for #{data.ID}.");

            if (
                data.StackLimitedNumber > 1 ||
                data.StackLimitedNumber == 1 && Main.Settings.RemoveForSingles
            )
                data.StackLimitedNumber = int.MaxValue;
        }

        [HarmonyPatch(typeof(ItemDataMgr), nameof(ItemDataMgr.GetItemBaseData))]
        public static class ItemDataMgrGetItemBaseData
        {
            public static void Postfix(ItemBaseConfData __result)
            {
                if (!Enabled) return;

                try
                {
                    RemoveStackLimit(__result);
                }
                catch (Exception exception)
                {
                    Main.Logger.Exception(exception);
                }
            }
        }

        [HarmonyPatch(typeof(ItemDataMgr), nameof(ItemDataMgr.GetItemBaseConfDataByIndex))]
        public static class ItemDataMgrGetItemBaseConfDataByIndex
        {
            public static void Postfix(ItemBaseConfData __result)
            {
                if (!Enabled) return;

                try
                {
                    RemoveStackLimit(__result);
                }
                catch (Exception exception)
                {
                    Main.Logger.Exception(exception);
                }
            }
        }
    }
}