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

        [HarmonyPatch(typeof(GiveGiftUICtr), "Start")]
        private static class GiveGiftUICtrStart
        {
            public static Actor ExecutingActor;

            private static void Prefix(Actor ___targetActor) => ExecutingActor = ___targetActor;

            private static void Postfix() => ExecutingActor = null;
        }

        [HarmonyPatch(typeof(ItemBag), nameof(ItemBag.GetAllItems))]
        private static class ItemBagGetAllItems
        {
            private static void Postfix(List<ItemObject> __result)
            {
                if (!Enabled) return;

                try
                {
                    var actor = GiveGiftUICtrStart.ExecutingActor;
                    if (actor == null) return;
                    var npcId = actor.InstanceId;

                    var knownGiftOptions = Main.Settings.ShowUnknownGiftOptions
                        ? new HashSet<int>()
                        : FavorUtility.GetGiftHistory(npcId).ToHashSet();

                    var items = __result.ToList();
                    __result.Clear();
                    __result.AddRange(
                        items.OrderByDescending(item => Main.Settings.ShowUnknownGiftOptions || knownGiftOptions.Contains(item.ItemDataId)
                            ? FavorUtility.GetFavorBehaviorInfo(npcId, item.ItemDataId).FavorValue
                            : 0
                        )
                    );
                }
                catch (Exception exception) { Main.Logger.Exception(exception); }
            }
        }
    }
}