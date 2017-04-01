namespace LaberBot.Xbox360Controller.Device.Linux
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.IO;
    using System.Threading;

    using log4net;

    [Export]
    internal class LinuxInputEventSource : IInputEventSource
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(LinuxInputEventSource));

        private volatile bool _running;

        public event EventHandler<InputEventArgs> InputRecieved;

        private readonly string _inputDevice;

        [ImportingConstructor]
        public LinuxInputEventSource(DeviceConfig config)
        {
            _inputDevice = config.Device;
        }

        public void Start()
        {
            Logger.DebugFormat("Using input device '{0}'", _inputDevice);
            _running = true;

            var thread = new Thread(ReadInput);
            thread.Start();
        }

        public void Stop()
        {
            _running = false;
        }

        private void ReadInput()
        {
            using (var stream = new FileStream(_inputDevice, FileMode.Open))
            using (var reader = new BinaryReader(stream))
            {
                var events = ReadInputMessages(reader);
                foreach (var input in events)
                {
                    RaiseInputRecieved(new InputEventArgs(input));
                }
            }
            
            _running = false;
        }

        private IEnumerable<InputEvent> ReadInputMessages(BinaryReader reader)
        {
            while (_running)
            {
                reader.ReadUInt32(); // read time

                var evt = new InputEvent
                {
                    Value = reader.ReadInt16(),
                    Type = (InputEvent.InputType)reader.ReadByte(),
                    Number = reader.ReadByte()
                };

                yield return evt;
            }
        }
        
        private void RaiseInputRecieved(InputEventArgs e)
        {
            InputRecieved?.Invoke(this, e);
        }
    }
}
