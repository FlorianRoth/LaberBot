namespace LaberBot.Xbox360Controller.Device
{
    using System;

    public interface IInputEventSource
    {
        event EventHandler<InputEventArgs> InputRecieved;

        void Start();

        void Stop();
    }
}