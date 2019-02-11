using System;
using System.ComponentModel;
using Harmony12;
using InControl;
using JetBrains.Annotations;


namespace Hotkeys.Patches
{
    [Description("Emulate controller with keyboard")]
    [HarmonyPatch(typeof(PlayerAction), nameof(PlayerAction.AddDefaultBinding), typeof(InputControlType))]
    public class EmulateControllerWithKeyboard: PlayerActionSet
    {

        #region Patch

        [UsedImplicitly]
        private static void Postfix(PlayerAction __instance, InputControlType control)
        {
            try
            {
                if (!(Instance.Target is EmulateControllerWithKeyboard emulate)) return;

                foreach (var source in emulate.ActionsToLink.GetValues(control))
                    InputLinker.Link(source, __instance);
            }
            catch (Exception exception) { Main.Logger.Exception(exception); }
        }

        private readonly Multimap<InputControlType, PlayerAction> ActionsToLink = new Multimap<InputControlType, PlayerAction>();

        #endregion

        public PlayerAction LeftStickLeft { get; }
        public PlayerAction LeftStickRight { get; }
        public PlayerAction LeftStickUp { get; }
        public PlayerAction LeftStickDown { get; }
        public PlayerAction LeftStickButton { get; }

        public PlayerAction RightStickLeft { get; }
        public PlayerAction RightStickRight { get; }
        public PlayerAction RightStickUp { get; }
        public PlayerAction RightStickDown { get; }
        public PlayerAction RightStickButton { get; }

        public PlayerAction DPadLeft { get; }
        public PlayerAction DPadRight { get; }
        public PlayerAction DPadUp { get; }
        public PlayerAction DPadDown { get; }

        public PlayerAction A { get; }
        public PlayerAction B { get; }
        public PlayerAction X { get; }
        public PlayerAction Y { get; }

        public PlayerAction LeftTrigger { get; }
        public PlayerAction RightTrigger { get; }
        public PlayerAction LeftBumper { get; }
        public PlayerAction RightBumper { get; }

        private EmulateControllerWithKeyboard()
        {
            ActionsToLink.Add(InputControlType.LeftStickLeft, LeftStickLeft = CreatePlayerAction("Left Stick ←"));
            ActionsToLink.Add(InputControlType.LeftStickRight, LeftStickRight = CreatePlayerAction("Left Stick →"));
            ActionsToLink.Add(InputControlType.LeftStickUp, LeftStickUp = CreatePlayerAction("Left Stick ↑"));
            ActionsToLink.Add(InputControlType.LeftStickDown, LeftStickDown = CreatePlayerAction("Left Stick ↓"));
            ActionsToLink.Add(InputControlType.LeftStickButton, LeftStickButton = CreatePlayerAction("Left Stick ●"));

            ActionsToLink.Add(InputControlType.RightStickLeft, RightStickLeft = CreatePlayerAction("Right Stick ←"));
            ActionsToLink.Add(InputControlType.RightStickRight, RightStickRight = CreatePlayerAction("Right Stick →"));
            ActionsToLink.Add(InputControlType.RightStickUp, RightStickUp = CreatePlayerAction("Right Stick ↑"));
            ActionsToLink.Add(InputControlType.RightStickDown, RightStickDown = CreatePlayerAction("Right Stick ↓"));
            ActionsToLink.Add(InputControlType.RightStickButton, RightStickButton = CreatePlayerAction("Right Stick ●"));

            ActionsToLink.Add(InputControlType.DPadLeft, DPadLeft = CreatePlayerAction("D-pad ←"));
            ActionsToLink.Add(InputControlType.DPadRight, DPadRight = CreatePlayerAction("D-pad →"));
            ActionsToLink.Add(InputControlType.DPadUp, DPadUp = CreatePlayerAction("D-pad ↑"));
            ActionsToLink.Add(InputControlType.DPadDown, DPadDown = CreatePlayerAction("D-pad ↓"));

            ActionsToLink.Add(InputControlType.Action1, A = CreatePlayerAction("A"));
            ActionsToLink.Add(InputControlType.Action2, B = CreatePlayerAction("B"));
            ActionsToLink.Add(InputControlType.Action3, X = CreatePlayerAction("X"));
            ActionsToLink.Add(InputControlType.Action4, Y = CreatePlayerAction("Y"));

            ActionsToLink.Add(InputControlType.LeftTrigger, LeftTrigger = CreatePlayerAction("Left Trigger"));
            ActionsToLink.Add(InputControlType.RightTrigger, RightTrigger = CreatePlayerAction("Right Trigger"));
            ActionsToLink.Add(InputControlType.LeftBumper, LeftBumper = CreatePlayerAction("Left Bumper"));
            ActionsToLink.Add(InputControlType.RightBumper, RightBumper = CreatePlayerAction("Right Bumper"));
        }

        private EmulateControllerWithKeyboard(BindingsMap<EmulateControllerWithKeyboard> bindingMap): this() => bindingMap.Apply(this);

        private static WeakReference Instance;

        public static EmulateControllerWithKeyboard GetOrCreate(BindingsMap<EmulateControllerWithKeyboard> bindingMap, bool enabled)
        {
            if (!(Instance?.Target is EmulateControllerWithKeyboard))
                Instance = new WeakReference(new EmulateControllerWithKeyboard(bindingMap));
            var result = (EmulateControllerWithKeyboard)Instance.Target;

            result.Enabled = enabled;
            return result;
        }
    }
}