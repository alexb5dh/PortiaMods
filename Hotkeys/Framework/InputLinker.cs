using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Harmony12;
using InControl;
using JetBrains.Annotations;
using UnityEngine;

namespace Hotkeys
{
    /// <summary>
    /// Links two <see cref="OneAxisInputControl"/>s, so that when "source" is triggered, "target" will also trigger.
    /// </summary>
    [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
    public static class InputLinker
    {
        [HarmonyPatch(typeof(OneAxisInputControl), nameof(OneAxisInputControl.UpdateWithValue))]
        private static class OneAxisPatch
        {
            [UsedImplicitly]
            private static void Prefix(OneAxisInputControl __instance, ref float value)
            {
                try
                {
                    var source = OneAxis.GetValues(__instance).LastOrDefault(_ => _.Value != 0);
                    if (source != null)
                    {
                        value = source.Value;
                    }
                }
                catch (Exception exception) { Main.Logger.Exception(exception); }
            }
        }

        [HarmonyPatch(typeof(TwoAxisInputControl), "UpdateWithAxes")]
        private static class TwoAxisPatch
        {
            [UsedImplicitly]
            private static void Prefix(TwoAxisInputControl __instance, ref float x, ref float y)
            {
                try
                {
                    var source = TwoAxis.GetValues(__instance).LastOrDefault(_ => _.Value != Vector2.zero);
                    if (source != null)
                    {
                        x = source.Value.x;
                        y = source.Value.y;
                    }
                }
                catch (Exception exception) { Main.Logger.Exception(exception); }
            }
        }

        private static readonly Multimap<OneAxisInputControl, OneAxisInputControl> OneAxis =
            new Multimap<OneAxisInputControl, OneAxisInputControl>();

        private static readonly Multimap<TwoAxisInputControl, TwoAxisInputControl> TwoAxis =
            new Multimap<TwoAxisInputControl, TwoAxisInputControl>();

        public static void Link(OneAxisInputControl source, OneAxisInputControl target) => OneAxis.Add(target, source);

        public static void Link(TwoAxisInputControl source, TwoAxisInputControl target) => TwoAxis.Add(target, source);
    }
}