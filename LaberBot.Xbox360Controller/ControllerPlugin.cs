namespace LaberBot.Xbox360Controller
{
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Linq;

    using log4net;

    using LaberBot.Bot;
    using LaberBot.Xbox360Controller.Device;

    [Export(typeof(IBotPlugin))]
    public class ControllerPlugin : IBotPlugin
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ControllerPlugin));

        private static readonly IDictionary<ControllerEventArgs.KeyCode, string> SoundMapping = new Dictionary<ControllerEventArgs.KeyCode, string>
                {
                    { ControllerEventArgs.KeyCode.A, "achja" },
                    { ControllerEventArgs.KeyCode.B, "gleiten" },
                    { ControllerEventArgs.KeyCode.X, "nyx" },
                    { ControllerEventArgs.KeyCode.Y, "Hallo" },
                    { ControllerEventArgs.KeyCode.L, "hafen" },
                    { ControllerEventArgs.KeyCode.R, "hass" },
                    { ControllerEventArgs.KeyCode.Back, "schanuze" },
                    { ControllerEventArgs.KeyCode.Start, "radar" },
                    { ControllerEventArgs.KeyCode.Up, "band" },
                    { ControllerEventArgs.KeyCode.Down, "fcb" },
                    { ControllerEventArgs.KeyCode.Left, "m1" },
                    { ControllerEventArgs.KeyCode.Right, "kohlen" }
                };

        private readonly ControllerOptions _options;

        private readonly IController _controller;

        private readonly IAudioPlayer _player;

        private readonly ISoundRepository _soundRepository;
        
        private ILaberBot _bot;

        [ImportingConstructor]
        public ControllerPlugin(
            ControllerOptions options,
            IController controller,
            IAudioPlayer player,
            ISoundRepository soundRepository)
        {
            _options = options;
            _controller = controller;
            _player = player;
            _soundRepository = soundRepository;
            controller.KeyPressed += OnKeyPressed;
        }

        public void Init(ILaberBot bot)
        {
            _bot = bot;
        }

        public void Run()
        {
            Logger.Info("Controller plugin configuration:");
            Logger.InfoFormat("  - Server: {0}", _options.Server);
            Logger.InfoFormat("  - User:   {0}", _options.User);

            Logger.Info("Controller mappings:");
            foreach (var mapping in SoundMapping)
            {
                Logger.InfoFormat("  - {0,-5} -> {1}", mapping.Key, mapping.Value);
            }
            
            _controller.Start();
        }
        
        private void OnKeyPressed(object sender, ControllerEventArgs args)
        {
            var key = args.Key;
            var state = args.State;

            if (state != InputEvent.State.Pressed)
            {
                return;
            }
            
            var server = _bot.FindServers(_options.Server).FirstOrDefault();
            if (null == server)
            {
                Logger.Error("Could not find server");
                return;
            }

            var user = server.FindUsers(_options.User).FirstOrDefault();
            if (user == null)
            {
                Logger.Error("Could not find user");
                return;
            }
            
            var channel = user.VoiceChannel;
            if (null == channel)
            {
                Logger.Error("Could not find voice channel");
                return;
            }

            var soundName = GetSound(key);
            if (null == soundName)
            {
                Logger.Error("Could not find sound");
                return;
            }

            var sound = _soundRepository.GetSoundFile(soundName);

            _player.PlayAsync(channel, sound.Path);
        }

        private string GetSound(ControllerEventArgs.KeyCode key)
        {
            string sound;
            SoundMapping.TryGetValue(key, out sound);

            return sound;
        }
    }
}
