using System;
using System.Collections.Generic;
using System.Linq;
using Harmony12;
using JetBrains.Annotations;
using Pathea.FavorSystemNs;

namespace Socialize.Patches
{
    public class ShowAllGifts
    {
        private static bool Enabled => Main.Enabled && Main.Settings.ShowAllGifts;

        [HarmonyPatch(typeof(FavorUtility), nameof(FavorUtility.GetGiftHistory))]
        private static class FavorUtilityGetGiftHistory
        {
            [UsedImplicitly]
            private static void Postfix(ref List<int> __result, int npcId)
            {
                if (!Enabled) return;

                try
                {
                    var favorItems = FavorUtility.GetDataBaseFavorItems(npcId, new List<FeeLevelEnum>
                            { FeeLevelEnum.Excellent, FeeLevelEnum.Like, FeeLevelEnum.DisLike, FeeLevelEnum.Hate }, 0)
                        .OrderByDescending(item => Math.Abs(FavorUtility.GetFavorBehaviorInfo(npcId, item).FavorValue));

                    __result = __result.Copy();
                    var addedGiftsSet = __result.ToHashSet();
                    __result.AddRange(favorItems.Where(item => addedGiftsSet.Add(item)));
                }
                catch (Exception exception) { Main.Logger.Exception(exception); }
            }
        }
    }
}