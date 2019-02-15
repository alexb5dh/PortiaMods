using Harmony12;
using JetBrains.Annotations;
using Pathea.ItemSystem;
using Pathea.UISystemNs;

namespace TweakIt.Patches
{
    /// <summary>
    /// Disables bag sorting after "Sort All" performed.
    /// </summary>
    public class DisableBagSortingForSortAll
    {
        private static bool Enabled => Main.Enabled;

        [HarmonyPatch(typeof(StoreageUICtr), nameof(StoreageUICtr.SortBagToStorage))]
        private static class StoreageUICtrSortBagToStorage
        {
            public static bool Executing;

            [UsedImplicitly]
            private static void Prefix() => Executing = true;

            [UsedImplicitly]
            private static void Postfix() => Executing = false;
        }

        [HarmonyPatch(typeof(ItemBag), nameof(ItemBag.SortTable))]
        private static class ItemBagSortTable
        {
            [UsedImplicitly]
            private static bool Prefix()
            {
                if (!Enabled) return true;

                return !StoreageUICtrSortBagToStorage.Executing;
            }
        }
    }
}