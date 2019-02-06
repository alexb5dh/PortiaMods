using System;
using System.Collections.Generic;
using System.Linq;
using Harmony12;
using Hont.ExMethod.Collection;
using Pathea.ActorNs;
using Pathea.FavorSystemNs;
using Pathea.ItemSystem;
using Pathea.UISystemNs;

namespace TweakIt.Patches
{
    internal static class OrderGiftsByPreference
    {
        private static bool Enabled => Main.Enabled && Main.Settings.OrderGiftsByPreference;

        private static Actor _currentActor;

        [HarmonyPatch(typeof(GiveGiftUICtr), "Start")]
        private static class GiveGiftUICtrStart
        {
            private static void Prefix(Actor ___targetActor) => _currentActor = ___targetActor;

            private static void Postfix() => _currentActor = null;
        }

        [HarmonyPatch(typeof(ItemBag), nameof(ItemBag.GetAllItems))]
        private static class ItemBagGetAllItems
        {
            private static void Postfix(List<ItemObject> __result)
            {
                if (!Enabled) return;

                try
                {
                    if (_currentActor == null) return;

                    var knownGiftOptions = Main.Settings.ShowUnknownGiftOptions
                        ? new HashSet<int>()
                        : FavorUtility.GetGiftHistory(_currentActor.InstanceId).ToHashSet();

                    Main.Logger.Log($"{knownGiftOptions.Count}");
                    Main.Logger.Log($"{_currentActor.ActorName}");

                    var items = __result.ToList();
                    __result.Clear();
                    __result.AddRange(
                        items.OrderByDescending(item => Main.Settings.ShowUnknownGiftOptions || knownGiftOptions.Contains(item.ItemDataId)
                            ? FavorUtility.GetFavorBehaviorInfo(_currentActor.InstanceId, item.ItemDataId).FavorValue
                            : 0
                        )
                    );
                }
                catch (Exception exception) { Main.Logger.Exception(exception); }
            }
        }
    }
}