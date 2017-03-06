namespace LaberBot.Bot.Commands
{
    using System;
    using System.ComponentModel.Composition;
    using System.Threading.Tasks;

    using Discord.Commands;

    [Export(typeof(IBotCommand))]
    public class PlayCommand : BotCommandBase
    {
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
            Parameter = new[] { Tuple.Create("file", ParameterType.Required) };
        }

        public override async Task ExecuteAsync(CommandEventArgs args)
        {
            var voiceChannel = args.User.VoiceChannel;
            if (null == voiceChannel)
            {
                return;
            }

            var sound = args.GetArg(0);
            var file = _soundRepository.GetSoundFile(sound);
            
            if (null == file)
            {
                await SendMessageToChannelAsync(args, $"Sound '{sound}' does not exist");
                return;
            }

            await _player.PlayAsync(voiceChannel, file);
        }
    }
}
