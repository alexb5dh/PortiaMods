using System;
using Harmony12;
using Pathea.FavorSystemNs;

namespace TweakIt.Patches
{
    /// <summary>
    /// Patches to show favor value in social tab.
    /// </summary>
    // ShowFavorValueChangeUITips
    internal static class DisplayFavorValue
    {
        private static bool Enabled => Main.Enabled && Main.Settings.ShowFavor;

        [HarmonyPatch(typeof(FavorUtility), "GetGradeName")]
        private static class FavorUtilityGetGradeName
        {
            private static int? GetNextFavorValue(FavorObject favor)
            {
                var relationship = FavorRelationshipData.GetRefData((int)favor.Relationship);
                if (relationship == null) return null;

                if (favor.FavorValue >= 0) return relationship.CanUpgrade ? relationship.upgradeValue : (int?)null;
                else return relationship.CanDowngrade ? relationship.downgradeValue : (int?)null;
            }

            private static string Format(FavorObject favorObj)
            {
                var favor = favorObj.FavorValue;
                var favorString = favor.ToString("+#;-#;0");

                var nextFavor = GetNextFavorValue(favorObj);
                if (nextFavor != null) favorString = $"{favorString} / {nextFavor}";

                if (favor > 0) favorString = favorString.Colored("green");
                else if (favor < 0) favorString = favorString.Colored("red");

                return favorString;
            }

            public static void Postfix(FavorObject favorObj, ref string __result)
            {
                if (!Enabled) return;

                try
                {
                    if (favorObj == null || __result == null) return;
                    __result = $"{__result} ({Format(favorObj)})";
                }
                catch (Exception exception) { Main.Logger.Exception(exception); }
            }
        }
    }
}