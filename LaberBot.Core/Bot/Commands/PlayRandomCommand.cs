namespace LaberBot.Bot.Commands
{
    using System;
    using System.ComponentModel.Composition;
    using System.Linq;
    using System.Threading.Tasks;

    using Discord.Commands;

    [Export(typeof(IBotCommand))]
    public class PlayRandomCommand : BotCommandBase
    {
        private readonly IAudioPlayer _player;

        private readonly ISoundRepository _soundRepository;

        private readonly Random _random;

        [ImportingConstructor]
        public PlayRandomCommand(
            IAudioPlayer player,
            ISoundRepository soundRepository)
            : base("playrnd", "Play a random sound")
        {
            _player = player;
            _soundRepository = soundRepository;
            _random = new Random();
        }

        public override async Task ExecuteAsync(CommandEventArgs args)
        {
            var voiceChannel = args.User.VoiceChannel;
            if (null == voiceChannel)
            {
                return;
            }

            var allSounds = _soundRepository.ListSounds().ToList();
            var index = _random.Next(0, allSounds.Count);
            var sound = allSounds[index];

            var file = _soundRepository.GetSoundFile(sound);

            await _player.PlayAsync(voiceChannel, file);
        }
    }
}
