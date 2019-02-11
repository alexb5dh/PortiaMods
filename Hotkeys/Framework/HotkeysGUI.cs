using System.Collections.Generic;
using Hotkeys.Extensions;
using InControl;
using UnityEngine;

namespace Hotkeys
{
    public static class HotkeysGUI
    {
        private static readonly Dictionary<PlayerActionSet, bool> InputDisplay = new Dictionary<PlayerActionSet, bool>();

        public static void Action(PlayerAction action)
        {
            if (action.ListenOptions == null)
            {
                action.ListenOptions = new BindingListenOptions
                {
                    MaxAllowedBindings = 3,
                    IncludeControllers = false,
                    OnBindingRejected = (_, binding, reason) =>
                        Main.Logger.Log($"Binding [{binding.Name}] for \"{action.Name}\" rejected because of {reason}.")
                };
            }

            GUI.enabled = action.Owner.Enabled;

            GUILayout.BeginHorizontal();

            GUILayout.Label(action.Name, GUILayout.Width(200));

            if (GUILayout.Button(action.IsListeningForBinding ? "Cancel" : "Add", GUILayout.Width(50)))
            {
                if (action.IsListeningForBinding) action.StopListeningForBinding();
                else action.ListenForBinding();
            }

            foreach (var binding in action.Bindings)
            {
                if (GUILayout.Button($"[{binding.Name}]", "Label", GUILayout.ExpandWidth(false)))
                    action.RemoveBinding(binding);
                GUILayout.Space(5);
            }

            GUI.enabled = true;

            GUILayout.EndHorizontal();
        }

        public static void Input(PlayerActionSet input)
        {
            if (!InputDisplay.TryGetValue(input, out var display))
                InputDisplay[input] = display = false;

            GUILayout.BeginHorizontal();
            input.Enabled = GUILayout.Toggle(input.Enabled, "", GUILayout.ExpandWidth(false));
            if (GUILayout.Button($"{(display ? "▲" : "▼")} <color=cyan><b>{input.Description()}</b></color>", "Label"))
                InputDisplay[input] = !InputDisplay[input];
            GUILayout.EndHorizontal();

            if (display)
            {
                GUILayout.Label("<i>Bindings are listed below in []. Click on a binding to remove it.</i>");
                foreach (var action in input.Actions) Action(action);
            }
        }

        public static void Inputs(IEnumerable<PlayerActionSet> inputs)
        {
            GUILayout.BeginVertical(OptionsGUI.Style);

            var first = true;
            foreach (var input in inputs)
            {
                Input(input);

                if (!first) GUILayout.Space(10);
                first = false;
            }

            GUILayout.EndVertical();
        }
    }
}