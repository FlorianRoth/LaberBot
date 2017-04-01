
namespace LaberBot.Xbox360Controller.Device
{
    using System;

    public class InputEventArgs : EventArgs
    {
        public InputEventArgs(InputEvent evt)
        {
            Event = evt;
        }

        public InputEvent Event { get; }
    }
}
