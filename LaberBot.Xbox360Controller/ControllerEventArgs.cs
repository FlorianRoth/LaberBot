namespace LaberBot.Xbox360Controller
{
    using System;

    using LaberBot.Xbox360Controller.Device;

    public class ControllerEventArgs : EventArgs
    {
        public enum KeyCode
        {
            A,
            B,
            X,
            Y,
            L,
            R,
            Start,
            Back,
            Up,
            Down,
            Left,
            Right
        }

        public KeyCode Key { get; }

        public InputEvent.State State { get; }

        public ControllerEventArgs(KeyCode key, InputEvent.State state)
        {
            Key = key;
            State = state;
        }
    }
}