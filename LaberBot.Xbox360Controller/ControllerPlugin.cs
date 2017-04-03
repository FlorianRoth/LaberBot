namespace LaberBot.Xbox360Controller
{
    using System.ComponentModel.Composition;
    using System.IO;
    using System.Linq;

    using log4net;

    using LaberBot.Bot;
    using LaberBot.Xbox360Controller.Device;

    using Newtonsoft.Json;

    [Export(typeof(IBotPlugin))]
    public class ControllerPlugin : IBotPlugin
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ControllerPlugin));
        
        private readonly ControllerOptions _options;

        private readonly IController _controller;

        private readonly IAudioPlayer _player;

        private readonly ISoundRepository _soundRepository;
        
        private ILaberBot _bot;

        private ControllerMapping _controllerMapping;

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

            var config = ReadConfig();
            _controllerMapping = new ControllerMapping(config);
        }

        private ControllerMappingConfiguration ReadConfig()
        {
            const string CONFIG_FILE = "controllermapping.json";

            if (false == File.Exists(CONFIG_FILE))
            {
                Logger.WarnFormat("Controller mapping file '{0}' not found. Using default configuration", CONFIG_FILE);
                return new ControllerMappingConfiguration();
            }

            using (var reader = new StreamReader(CONFIG_FILE))
            using (var jsonReader = new JsonTextReader(reader))
            {

                var serializer = new JsonSerializer();
                var config = serializer.Deserialize<ControllerMappingConfiguration>(jsonReader);

                return config;
            }
        }

        public void Run()
        {
            Logger.Info("Controller plugin configuration:");
            Logger.InfoFormat("  - Server: {0}", _options.Server);
            Logger.InfoFormat("  - User:   {0}", _options.User);

            Logger.Info("Controller mappings:");
            foreach (var mapping in _controllerMapping)
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
            _controllerMapping.TryGetValue(key, out sound);

            return sound;
        }
    }
}
