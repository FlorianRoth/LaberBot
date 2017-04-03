namespace LaberBot.Xbox360Controller
{
    using System.Collections;
    using System.Collections.Generic;

    public class ControllerMapping : IReadOnlyDictionary<ControllerEventArgs.KeyCode, string>
    {
        private readonly IDictionary<ControllerEventArgs.KeyCode, string> _data;
        

        public int Count => _data.Count;

        public IEnumerable<ControllerEventArgs.KeyCode> Keys => _data.Keys;

        public IEnumerable<string> Values => _data.Values;

        public string this[ControllerEventArgs.KeyCode key] => _data[key];


        public ControllerMapping(ControllerMappingConfiguration config)
        {
            _data = new Dictionary<ControllerEventArgs.KeyCode, string>();

            _data[ControllerEventArgs.KeyCode.A] = config.A;
            _data[ControllerEventArgs.KeyCode.B] = config.B;
            _data[ControllerEventArgs.KeyCode.X] = config.X;
            _data[ControllerEventArgs.KeyCode.Y] = config.Y;
            _data[ControllerEventArgs.KeyCode.L] = config.L;
            _data[ControllerEventArgs.KeyCode.R] = config.R;
            _data[ControllerEventArgs.KeyCode.Back] = config.Back;
            _data[ControllerEventArgs.KeyCode.Start] = config.Start;
            _data[ControllerEventArgs.KeyCode.Up] = config.Up;
            _data[ControllerEventArgs.KeyCode.Down] = config.Down;
            _data[ControllerEventArgs.KeyCode.Left] = config.Left;
            _data[ControllerEventArgs.KeyCode.Right] = config.Right;
        }
        
        public bool ContainsKey(ControllerEventArgs.KeyCode key)
        {
            return _data.ContainsKey(key);
        }

        public bool TryGetValue(ControllerEventArgs.KeyCode key, out string value)
        {
            return _data.TryGetValue(key, out value);
        }
        
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<KeyValuePair<ControllerEventArgs.KeyCode, string>> GetEnumerator()
        {
            return _data.GetEnumerator();
        }
    }
}
