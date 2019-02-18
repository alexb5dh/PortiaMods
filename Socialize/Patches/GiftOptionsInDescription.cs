using System;
using System.Collections.Generic;
using System.Linq;
using Harmony12;
using JetBrains.Annotations;
using Pathea.FavorSystemNs;
using Pathea.ItemSystem;
using Pathea.ModuleNs;
using Pathea.NpcRepositoryNs;

namespace Socialize.Patches
{
    internal static class GiftOptionsInDescription
    {
        private static bool Enabled => Main.Enabled && Main.Settings.ShowGiftOptions;

        private static bool IncludeGiftOption(GiveGiftResult favor, NpcData npc, int itemId) =>
            (npc.Interact & InteractType.GiveGift) != 0 &&
            favor.FeeLevel.In(FeeLevelEnum.Excellent, FeeLevelEnum.Like) &&
            FavorUtility.GetGiftHistory(npc.id).Contains(itemId);

        private static string FormatGiftOption(GiveGiftResult favor, NpcData npc)
        {
            var result = $"{npc.Name}({favor.FavorValue:+#;-#;0})";

            switch (favor.FeeLevel)
            {
                case FeeLevelEnum.Excellent: return result.Colored("#2ecc40").Bold();
                case FeeLevelEnum.Like: return result.Colored("#2ecc40");
                case FeeLevelEnum.DisLike: return result.Colored("#85144b");
                case FeeLevelEnum.Hate: return  result.Colored("#85144b").Bold();
                default: return result.Colored("white");
            }
        }

        private static IEnumerable<KeyValuePair<NpcData, GiveGiftResult>> GetGiftOptions(int itemId)
        {
            foreach (var npc in Module<NpcRepository>.Self.NpcInstanceDatas ?? new List<NpcData>(0))
            {
                var favor = FavorUtility.GetFavorBehaviorInfo(npc.id, itemId);
                if (IncludeGiftOption(favor, npc, itemId))
                    yield return KeyValuePair.Create(npc, favor);
            }
        }

        [HarmonyPatch(typeof(ItemCommon), nameof(ItemCommon.Description_1), MethodType.Getter)]
        private static class ItemCommonDescription1
        {
            [UsedImplicitly]
            public static void Postfix(ItemCommon __instance, ref string __result)
            {
                if (!Enabled) return;

                try
                {
                    var giftOptions = GetGiftOptions(__instance.ID)
                        .OrderByDescending(p => p.Value.FeeLevel)
                        .ThenByDescending(p => p.Value.FavorValue)
                        .Distinct(p => p.Key)
                        .ToList();

                    if (giftOptions.Any())
                    {
                        __result = __result == null ? "" : __result + "\n\n";
                        __result += @"Gifting: ".Colored("#7fdbff");
                        __result += giftOptions.Select(p => FormatGiftOption(p.Value, p.Key)).StringJoin(", ");
                    }
                }
                catch (Exception exception) { Main.Logger.Exception(exception); }
            }
        }
    }
}