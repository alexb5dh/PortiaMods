using System;
using System.Collections.Generic;
using System.Globalization;
using Harmony12;
using Pathea.HomeNs;
using Pathea.UISystemNs;

namespace TweakIt.Patches
{
    /// <summary>
    /// Patches to sort chests by name.
    /// </summary>
    internal static class SortChestsByName
    {
        private static readonly IComparer<StorageUnit> StorageComparer = new SelectorComparer<StorageUnit, string>(
            unit => unit.StorageName, new NaturalComparer(CultureInfo.InvariantCulture, CompareOptions.OrdinalIgnoreCase)
        );

        private static bool Enabled => Main.Enabled && Main.Settings.SortChestsByName;

        [HarmonyPatch(typeof(List<StorageUnit>), new[] { typeof(StorageUnit) })] // generic type
        [HarmonyPatch(nameof(List<StorageUnit>.Add), typeof(StorageUnit))] // method
        private static class ListAdd
        {
            private static bool Prefix(List<StorageUnit> __instance, StorageUnit item)
            {
                // fix for https://github.com/pardeike/Harmony/issues/156
                if (__instance?.GetType() != typeof(List<StorageUnit>)) return true;

                if (!Enabled) return true;

                try
                {
                    __instance.InsertSorted(item, StorageComparer);
                    Main.Logger.Debug($"Inserted [{item.StorageName}] in order.");
                    return false;
                }
                catch (Exception exception) { Main.Logger.Exception(exception); }

                return true;
            }
        }

        /// <summary>
        /// Reinserts chest after renaming.
        /// </summary>
        [HarmonyPatch(typeof(StorageUnit), nameof(StorageUnit.StorageName), MethodType.Setter)]
        private static class StorageUnitStorageNamePatch
        {
            public static void Prefix(StorageUnit __instance, out string __state)
            {
                __state = null;

                if (!Enabled) return;

                try
                {
                    __state = __instance.StorageName;
                }
                catch (Exception exception) { Main.Logger.Exception(exception); }
            }

            public static void Postfix(StorageUnit __instance, string __state, List<StorageUnit> ___globalStorages, string value)
            {
                if (!Enabled) return;

                try
                {
                    if (___globalStorages == null) return;

                    string oldName = __state, newName = value;
                    if (oldName.EqualsOrdinal(newName)) return;

                    if (___globalStorages.Remove(__instance))
                    {
                        ___globalStorages.Add(__instance);
                        Main.Logger.Debug($"Reinserted chest renamed from [{oldName}] to [{newName}].");
                    }
                }
                catch (Exception exception) { Main.Logger.Exception(exception); }
            }
        }

        /// <summary>
        /// Correctly updates current chest index after renaming in chest UI.
        /// </summary>
        [HarmonyPatch(typeof(StoreageUICtr), "EndEditName")]
        private static class StoreageUICtrEndEditName
        {
            public static void Prefix(int ___curStorageGlobalIndex, out StorageUnit __state)
            {
                __state = null;

                if (!Enabled) return;

                try
                {
                    __state = StorageUnit.GetStorageByGlobalIndex(___curStorageGlobalIndex);
                }
                catch (Exception exception) { Main.Logger.Exception(exception); }
            }

            public static void Postfix(StoreageUICtr __instance, StorageUnit __state)
            {
                if (!Enabled) return;

                try
                {
                    if (__state == null) return;

                    var newIndex = StorageUnit.StorageGlobalIndex(__state);
                    __instance.SetDropDownValue(newIndex);
                    Main.Logger.Debug($"Changed selection index for chest [{__state.StorageName}] to {newIndex}.");
                }
                catch (Exception exception) { Main.Logger.Exception(exception); }
            }
        }
    }
}