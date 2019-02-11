using System;
using System.ComponentModel;
using Harmony12;
using InControl;
using JetBrains.Annotations;
using UnityEngine;

namespace Hotkeys.Patches
{
    [Description("Navigate with keyboard (menu, inventory, etc.)")]
    public class NavigateWithKeyboard: PlayerActionSet
    {

        #region Patch

        [HarmonyPatch(typeof(InControlInputModule), "UpdateInputState")]
        private static class InControlInputModuleConstructor
        {
            [UsedImplicitly]
            private static void Prefix(InControlInputModule __instance, ref TwoAxisInputControl ___direction)
            {
                try
                {
                    if (!(Instance.Target is NavigateWithKeyboard navigate)) return;

                    // avoid overwriting controller movement/presses
                    __instance.MoveAction = ___direction.Vector == Vector2.zero ? navigate.Move : null;
                    __instance.SubmitAction = !__instance.SubmitButton().IsPressed ? navigate.Submit : null;
                    __instance.CancelAction = !__instance.CancelButton().IsPressed ? navigate.Cancel : null;
                }
                catch (Exception exception) { Main.Logger.Exception(exception); }
            }

        }

        #endregion

        public PlayerAction Up { get; }
        public PlayerAction Down { get; }
        public PlayerAction Right { get; }
        public PlayerAction Left { get; }
        private PlayerTwoAxisAction Move { get; }

        public PlayerAction Submit { get; }
        public PlayerAction Cancel { get; }

        private NavigateWithKeyboard()
        {
            Up = CreatePlayerAction("↑");
            Down = CreatePlayerAction("↓");
            Right = CreatePlayerAction("→");
            Left = CreatePlayerAction("←");
            Move = CreateTwoAxisPlayerAction(Left, Right, Down, Up);
            Submit = CreatePlayerAction("Select / Submit");
            Cancel = CreatePlayerAction("Cancel / Esc");
        }

        public NavigateWithKeyboard(BindingsMap<NavigateWithKeyboard> bindingsMap): this() => bindingsMap.Apply(this);

        private static WeakReference Instance;

        public static NavigateWithKeyboard GetOrCreate(BindingsMap<NavigateWithKeyboard> bindingMap, bool enabled)
        {
            if (!(Instance?.Target is NavigateWithKeyboard))
                Instance = new WeakReference(new NavigateWithKeyboard(bindingMap));
            var result = (NavigateWithKeyboard)Instance.Target;

            result.Enabled = enabled;
            return result;
        }

    }
}