using System;
using System.Collections.Generic;
using Harmony12;
using JetBrains.Annotations;
using Pathea;
using Pathea.GuildRanking;
using Pathea.ItemSystem;
using Pathea.ModuleNs;
using Pathea.UISystemNs;
using TMPro;

namespace TweakIt.Patches
{
    public static class DetailedNotifications
    {
        private static bool Enabled => Main.Enabled && Main.Settings.DetailedNotifications;

        [HarmonyPatch(typeof(MessageUITips_Number), nameof(MessageUITips_Number.SetPanel),
            typeof(Action<MessageUITipsPanelBase>), typeof(float), typeof(string), typeof(ITipShow), typeof(bool))]
        private static class MessageUITipsNumber
        {
            public static readonly Dictionary<TextMeshProUGUI, ITipShow> TipInfoByTextField = new Dictionary<TextMeshProUGUI, ITipShow>();

            [UsedImplicitly]
            private static void Postfix(TextChangeEffectNumberCollect ___textChangeEffect, ITipShow tipShow)
            {
                try
                {
                    TipInfoByTextField[___textChangeEffect.countText] = tipShow;
                }
                catch (Exception exception) { Main.Logger.Exception(exception); }
            }
        }

        [HarmonyPatch(typeof(TMP_Text), nameof(TMP_Text.text), MethodType.Setter)]
        private static class TextChangeEffectNumberCollectCountChangeTween
        {
            [UsedImplicitly]
            public static void Prefix(TextMeshProUGUI __instance, ref string value)
            {
                if (!Enabled) return;

                try
                {
                    if (!MessageUITipsNumber.TipInfoByTextField.TryGetValue(__instance, out var tip))
                        return;

                    switch (tip)
                    {
                        case ItemObject item:
                            value = FormatItem(__instance, value, item);
                            break;
                        case MoneyTip money:
                            value = FormatMoney(__instance, value, money);
                            break;
                        case Player.ExpTip experience:
                            value = FormatExperience(__instance, value, experience);
                            break;
                        case ReputationChangeValue reputation:
                            value = FormatReputation(__instance, value, reputation);
                            break;
                    }
                }
                catch (Exception exception) { Main.Logger.Exception(exception); }
            }
        }

        private static string FormatItem(TextMeshProUGUI textField, string number, ItemObject item)
        {
            var itemCount = Module<Player>.Self.bag.GetItemCount(item.ItemDataId);

            var itemCountString = $"{itemCount}".Colored(textField.color);
            return number + $" (in bag: {itemCountString})".Colored("white");
        }

        private static string FormatMoney(TextMeshProUGUI textField, string number, MoneyTip money)
        {
            var moneyCount = Module<Player>.Self.bag.Money;

            var moneyString = $"{moneyCount}".Colored(textField.color);
            return number + $" (total: {moneyString})".Colored("white");
        }

        private static string FormatExperience(TextMeshProUGUI textField, string number, Player.ExpTip experience)
        {
            var level = Module<Player>.Self.actor.Level;

            var amountLeftString = $"{level.nextLevelExp - level.exp}".Colored(textField.color);
            return number + $" (level in: {amountLeftString})".Colored("white");
        }

        private static string FormatReputation(TextMeshProUGUI textField, string number, ReputationChangeValue reputation)
        {
            var workshop = Module<GuildRankingManager>.Self.GetPlayerWorkshop();
            var nextLevel = ReputationValueData.GetData(workshop.level + 1);
            if (nextLevel == null) return number;

            var nextLevelString = nextLevel.levelShow.Colored(textField.color);
            var amountLeftString = $"{nextLevel.upgradeValue - workshop.curValue}".Colored(textField.color);
            return number + $" (rank {nextLevelString} in {amountLeftString})".Colored("white");
        }
    }
}