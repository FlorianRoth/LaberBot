namespace LaberBot.Xbox360Controller
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;

    using LaberBot.Xbox360Controller.Device;

    [Export(typeof(IController))]
    public class Xbox360Controller : IController
    {
        private static readonly IDictionary<byte, ControllerEventArgs.KeyCode> KeyMapping = new Dictionary<byte, ControllerEventArgs.KeyCode>
                {
                    { 0x00, ControllerEventArgs.KeyCode.A },
                    { 0x01, ControllerEventArgs.KeyCode.B },
                    { 0x02, ControllerEventArgs.KeyCode.X },
                    { 0x03, ControllerEventArgs.KeyCode.Y },
                    { 0x04, ControllerEventArgs.KeyCode.L },
                    { 0x05, ControllerEventArgs.KeyCode.R },
                    { 0x06, ControllerEventArgs.KeyCode.Back },
                    { 0x07, ControllerEventArgs.KeyCode.Start },
                    { 0x0D, ControllerEventArgs.KeyCode.Up },
                    { 0x0E, ControllerEventArgs.KeyCode.Down },
                    { 0x0B, ControllerEventArgs.KeyCode.Left },
                    { 0x0C, ControllerEventArgs.KeyCode.Right }
                };

        private readonly IInputEventSource _inputEventSource;

        public event EventHandler<ControllerEventArgs> KeyPressed; 

        [ImportingConstructor]
        public Xbox360Controller(IInputEventSource inputEventSource)
        {
            _inputEventSource = inputEventSource;
            _inputEventSource.InputRecieved += OnInputRecieved;
        }

        public void Start()
        {
            _inputEventSource.Start();
        }

        public void Stop()
        {
            _inputEventSource.Stop();
        }

        private void OnInputRecieved(object sender, InputEventArgs inputEventArgs)
        {
            var value = inputEventArgs.Event;
            if (false == IsButton(value))
            {
                return;
            }

            var buttonId = value.Number;
            var state = (InputEvent.State)value.Value;
            
            ControllerEventArgs.KeyCode code;
            if (KeyMapping.TryGetValue(buttonId, out code))
            {
                var args = new ControllerEventArgs(code, state);
                RaiseKeyPressed(args);
            }
        }

        private bool IsButton(InputEvent ev)
        {
            var type = (byte)ev.Type;

            if (CheckFlag(type, InputEvent.InputType.Init))
            {
                return false;
            }

            return CheckFlag(type, InputEvent.InputType.Button);
        }

        private bool CheckFlag(byte value, InputEvent.InputType t)
        {
            return (value & (byte)t) == (byte)t;
        }

        private void RaiseKeyPressed(ControllerEventArgs e)
        {
            KeyPressed?.Invoke(this, e);
        }
    }
}
