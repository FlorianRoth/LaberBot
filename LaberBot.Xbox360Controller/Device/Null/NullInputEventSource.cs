namespace LaberBot.Xbox360Controller.Device.Null
{
    using System;
    using System.ComponentModel.Composition;
    
    [Export(typeof(NullInputEventSource))]
    internal class NullInputEventSource : IInputEventSource
    {
#pragma warning disable 67
        public event EventHandler<InputEventArgs> InputRecieved;
#pragma warning restore 67

        public void Start()
        {
        }

        public void Stop()
        {
        }
    }
}
