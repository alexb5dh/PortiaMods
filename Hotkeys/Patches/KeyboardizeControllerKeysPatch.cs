using System;
using Harmony12;
using InControl;
using JetBrains.Annotations;

namespace Hotkeys.Patches
{
    [HarmonyPatch(typeof(PlayerAction), nameof(PlayerAction.AddDefaultBinding), typeof(InputControlType))]
    public class KeyboardizeControllerKeysPatch
    {
        [UsedImplicitly]
        public static void Postfix(PlayerAction __instance, InputControlType control)
        {
            if (!Main.Enabled) return;

            try
            {
                switch (control)
                {
                    case InputControlType.Action1: // Confirm
                    {
                        __instance.AddDefaultBinding(Key.Space);
                        __instance.AddDefaultBinding(Key.Return);
                        break;
                    }
                    case InputControlType.Action2: // Jump, Stop, Cancel, Delete
                    {
                        __instance.AddDefaultBinding(Key.Escape);
                        __instance.AddDefaultBinding(Key.Backspace);
                        break;
                    }
                    case InputControlType.Action3: // Attack, Add fuel, Take photo
                    {
                        __instance.AddDefaultBinding(Key.F);
                        __instance.AddDefaultBinding(Key.End);
                        break;
                    }
                    case InputControlType.Action4: // Stop, Cancel, Reset
                    {
                        __instance.AddDefaultBinding(Key.C);
                        __instance.AddDefaultBinding(Key.Insert);
                        break;
                    }
                    case InputControlType.LeftTrigger:
                    {
                        __instance.AddDefaultBinding(Key.Q);
                        break;
                    }
                    case InputControlType.RightTrigger:
                    {
                        __instance.AddDefaultBinding(Key.E);
                        break;
                    }
                    case InputControlType.LeftBumper:
                    {
                        __instance.AddDefaultBinding(Key.LeftControl, Key.Q);
                        break;
                    }
                    case InputControlType.RightBumper:
                    {
                        __instance.AddDefaultBinding(Key.LeftControl, Key.E);
                        break;
                    }
                    case InputControlType.RightStickUp:
                    {
                        __instance.AddDefaultBinding(Key.LeftControl, Key.W);
                        break;
                    }
                    case InputControlType.RightStickDown:
                    {
                        __instance.AddDefaultBinding(Key.LeftControl, Key.S);
                        break;
                    }
                    case InputControlType.RightStickLeft:
                    {
                        __instance.AddDefaultBinding(Key.LeftControl, Key.A);
                        break;
                    }
                    case InputControlType.RightStickRight:
                    {
                        __instance.AddDefaultBinding(Key.LeftControl, Key.D);
                        break;
                    }
                    case InputControlType.LeftStickUp:
                    {
                        __instance.AddDefaultBinding(Key.W);
                        break;
                    }
                    case InputControlType.LeftStickDown:
                    {
                        __instance.AddDefaultBinding(Key.S);
                        break;
                    }
                    case InputControlType.LeftStickLeft:
                    {
                        __instance.AddDefaultBinding(Key.A);
                        break;
                    }
                    case InputControlType.LeftStickRight:
                    {
                        __instance.AddDefaultBinding(Key.D);
                        break;
                    }
                }
            }
            catch (Exception exception) { Main.Logger.Exception(exception); }
        }
    }
}