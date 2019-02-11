using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Hotkeys.Patches;
using InControl;
using UnityModManagerNet;
using static InControl.Key;
using Keys = InControl.KeyBindingSource;

namespace Hotkeys
{
    [SuppressMessage("ReSharper", "CoVariantArrayConversion")]
    public class Settings: UnityModManager.ModSettings, IXmlSerializable
    {
        public static readonly PlayerActionSet[] Inputs =
        {
            NavigateWithKeyboard.GetOrCreate(new BindingsMap<NavigateWithKeyboard>
            {
                { _ => _.Up, new[] { new Keys(W), new Keys(UpArrow) } },
                { _ => _.Down, new[] { new Keys(S), new Keys(DownArrow) } },
                { _ => _.Right, new[] { new Keys(D), new Keys(RightArrow) } },
                { _ => _.Left, new[] { new Keys(A), new Keys(LeftArrow) } },
                { _ => _.Submit, new[] { new Keys(Space), new Keys(Return) } },
                { _ => _.Cancel, new[] { new Keys(Escape), new Keys(Backspace) } }
            }, enabled: true),

            EmulateControllerWithKeyboard.GetOrCreate(new BindingsMap<EmulateControllerWithKeyboard>
            {
                { _ => _.A, new[] { new Keys(Space), new Keys(Return) } },
                { _ => _.B, new[] { new Keys(Escape), new Keys(Backspace) } },
                { _ => _.X, new[] { new Keys(F), new Keys(End) } },
                { _ => _.Y, new[] { new Keys(C), new Keys(Insert) } },
                { _ => _.LeftTrigger, new[] { new Keys(Q) } },
                { _ => _.RightTrigger, new[] { new Keys(E) } },
                { _ => _.LeftBumper, new[] { new Keys(LeftControl, Q) } },
                { _ => _.RightBumper, new[] { new Keys(LeftControl, E) } },
                { _ => _.RightStickUp, new[] { new Keys(LeftControl, W) } },
                { _ => _.RightStickDown, new[] { new Keys(LeftControl, S) } },
                { _ => _.RightStickLeft, new[] { new Keys(LeftControl, A) } },
                { _ => _.RightStickRight, new[] { new Keys(LeftControl, D) } },
                { _ => _.LeftStickUp, new[] { new Keys(W) } },
                { _ => _.LeftStickDown, new[] { new Keys(S) } },
                { _ => _.LeftStickLeft, new[] { new Keys(A) } },
                { _ => _.LeftStickRight, new[] { new Keys(D) } }
            }, enabled: true)
        };

        public override void Save(UnityModManager.ModEntry modEntry) => Save(this, modEntry);

        #region Implementation of IXmlSerializable

        public XmlSchema GetSchema() => null;

        public void ReadXml(XmlReader reader)
        {
            try
            {
                var inputsByTypeName = Inputs.ToDictionary(input => input.GetType().Name);
                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        var typeName = reader.Name;
                        if (inputsByTypeName.TryGetValue(typeName, out var input))
                        {
                            input.Enabled = bool.Parse(reader.GetAttribute(nameof(input.Enabled)) ?? "false");
                            input.Load(reader.ReadElementContentAsString());
                        }
                    }
                }
            }
            catch (Exception exception) { Main.Logger.Exception(exception); }
        }

        public void WriteXml(XmlWriter writer)
        {
            try
            {
                foreach (var input in Inputs)
                {
                    writer.WriteStartElement(input.GetType().Name);
                    writer.WriteAttributeString(nameof(input.Enabled), $"{input.Enabled}");
                    writer.WriteString(input.Save());
                    writer.WriteEndElement();
                }
            }
            catch (Exception exception) { Main.Logger.Exception(exception); }
        }

        #endregion

    }
}