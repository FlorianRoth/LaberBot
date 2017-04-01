namespace LaberBot.Xbox360Controller.Device
{
    using System;
    using System.ComponentModel.Composition;
    using System.ComponentModel.Composition.Hosting;

    using log4net;

    using LaberBot.Xbox360Controller.Device.Linux;
    using LaberBot.Xbox360Controller.Device.Null;

    [Export(typeof(IInputEventSource))]
    internal class InputEventSourceProxy : IInputEventSource
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(InputEventSourceProxy));

        private readonly IInputEventSource _source;

        public event EventHandler<InputEventArgs> InputRecieved
        {
            add
            {
                _source.InputRecieved += value;
            }

            remove
            {
                _source.InputRecieved -= value;
            }
        }

        [ImportingConstructor]
        public InputEventSourceProxy(CompositionContainer container)
        {
            if (Environment.OSVersion.Platform == PlatformID.Unix)
            {
                _source = CreateSource<LinuxInputEventSource>(container);
            }
            else
            {
                _source = CreateSource<NullInputEventSource>(container);
            }
        }

        private static IInputEventSource CreateSource<T>(CompositionContainer container) where T : IInputEventSource
        {
            Logger.DebugFormat("Using {0}", typeof(T).Name);
            return container.GetExportedValue<T>();
        }

        public void Start()
        {
            _source.Start();
        }

        public void Stop()
        {
            _source.Stop();
        }
    }
}
