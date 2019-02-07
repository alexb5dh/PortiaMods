using System;
using Harmony12;
using InControl;
using JetBrains.Annotations;

namespace Hotkeys.Patches
{
    [HarmonyPatch(typeof(InControlInputModule), MethodType.Constructor)]
    public static class KeyboardizeControllerNavigationPatch
    {
        [UsedImplicitly]
        public static void Postfix(InControlInputModule __instance)
        {
            try
            {
                var input = KeyboardNavigationInput.CreateWithDefaultBindings();

                __instance.MoveAction = input.Move;
                __instance.SubmitAction = input.Submit;
                __instance.CancelAction = input.Cancel;

                input.Enabled = true;
            }
            catch (Exception exception) { Main.Logger.Exception(exception); }
        }

        public class KeyboardNavigationInput: PlayerActionSet
        {
            public readonly PlayerAction Up;
            public readonly PlayerAction Down;
            public readonly PlayerAction Right;
            public readonly PlayerAction Left;
            public readonly PlayerTwoAxisAction Move;
            public readonly PlayerAction Submit;
            public readonly PlayerAction Cancel;

            public KeyboardNavigationInput()
            {
                Up = CreatePlayerAction(nameof(Up));
                Down = CreatePlayerAction(nameof(Down));
                Right = CreatePlayerAction(nameof(Right));
                Left = CreatePlayerAction(nameof(Left));
                Move = CreateTwoAxisPlayerAction(Left, Right, Down, Up);
                Submit = CreatePlayerAction(nameof(Submit));
                Cancel = CreatePlayerAction(nameof(Cancel));
            }

            public static KeyboardNavigationInput CreateWithDefaultBindings()
            {
                var input = new KeyboardNavigationInput();

                input.Up.AddDefaultBinding(Key.W);
                input.Up.AddDefaultBinding(Key.UpArrow);

                input.Down.AddDefaultBinding(Key.S);
                input.Down.AddDefaultBinding(Key.DownArrow);

                input.Right.AddDefaultBinding(Key.D);
                input.Right.AddDefaultBinding(Key.RightArrow);

                input.Left.AddDefaultBinding(Key.A);
                input.Left.AddDefaultBinding(Key.LeftArrow);

                input.Submit.AddDefaultBinding(Key.Space);
                input.Submit.AddDefaultBinding(Key.Return);

                input.Cancel.AddDefaultBinding(Key.Escape);
                input.Cancel.AddDefaultBinding(Key.Backspace);

                return input;
            }
        }

    }
}