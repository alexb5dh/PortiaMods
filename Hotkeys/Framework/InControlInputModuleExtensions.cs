using InControl;

namespace Hotkeys
{
    public static class InControlInputModuleExtensions
    {
        public static InputControl SubmitButton(this InControlInputModule @this) => @this.Device.GetControl((InputControlType)@this.submitButton);

        public static InputControl CancelButton(this InControlInputModule @this) => @this.Device.GetControl((InputControlType)@this.cancelButton);
    }
}