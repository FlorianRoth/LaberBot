namespace LaberBot.Bot.Commands
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Linq;
    using System.Threading.Tasks;

    using Discord.Commands;

    [Export(typeof(IBotCommand))]
    public class PlayRandomCommand : BotCommandBase
    {
        private const string ARG_GROUP = "group";

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

            Parameter = new[] { Tuple.Create(ARG_GROUP, ParameterType.Optional) };
        }

        public override async Task ExecuteAsync(CommandEventArgs args)
        {
            var voiceChannel = args.User.VoiceChannel;
            if (null == voiceChannel)
            {
                return;
            }

            var group = GetGroup(args);
            var allSounds = GetSoundsForGroup(group).ToList();

            if (0 == allSounds.Count)
            {
                await SendMessageToChannelAsync(args, $"No sounds available for group '{group}'");
                return;
            }

            var index = _random.Next(0, allSounds.Count);
            var sound = allSounds[index];
            
            await _player.PlayAsync(voiceChannel, sound.Path);
        }

        private string GetGroup(CommandEventArgs args)
        {
            var group = args.GetArg(ARG_GROUP);

            return string.IsNullOrEmpty(group) ? null : group;
        }

        private IEnumerable<ISoundFile> GetSoundsForGroup(string group)
        {
            var sounds = _soundRepository.ListSounds();

            if (null == group)
            {
                return sounds;
            }

            return sounds.Where(s => s.Group == group);
        }
    }
}
