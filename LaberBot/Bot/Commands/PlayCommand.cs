namespace LaberBot.Bot.Commands
{
    using System;
    using System.ComponentModel.Composition;
    using System.Linq;
    using System.Threading.Tasks;

    using Discord;
    using Discord.Commands;

    [Export(typeof(IBotCommand))]
    public class PlayCommand : BotCommandBase
    {
        private const string ARG_FILE = "file";

        private const string ARG_USER = "user";

        private readonly IAudioPlayer _player;

        private readonly ISoundRepository _soundRepository;

        [ImportingConstructor]
        public PlayCommand(
            IAudioPlayer player,
            ISoundRepository soundRepository)
            : base("play", "Play a sound")
        {
            _player = player;
            _soundRepository = soundRepository;
            Parameter = new[]
            {
                Tuple.Create(ARG_FILE, ParameterType.Required),
                Tuple.Create(ARG_USER, ParameterType.Optional)
            };
        }

        public override async Task ExecuteAsync(CommandEventArgs args)
        {
            var sound = args.GetArg(ARG_FILE);
            var file = _soundRepository.GetSoundFile(sound);
            if (null == file)
            {
                await SendMessageToChannelAsync(args, $"Sound '{sound}' does not exist");
                return;
            }

            var voiceChannel = GetVoiceChannel(args);
            if (null == voiceChannel)
            {
                await SendMessageToChannelAsync(args, "No voice channel could be found");
                return;
            }
            
            await _player.PlayAsync(new DiscordChannel(voiceChannel), file.Path);
        }

        private Channel GetVoiceChannel(CommandEventArgs args)
        {
            var userName = args.GetArg(ARG_USER);

            if (string.IsNullOrEmpty(userName))
            {
                return args.User.VoiceChannel;
            }

            var user = args.Server.FindUsers(userName, true).FirstOrDefault();

            return user?.VoiceChannel;
        }
    }
}
