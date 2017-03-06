namespace LaberBot.Bot.Commands
{
    using System.ComponentModel.Composition;
    using System.Text;
    using System.Threading.Tasks;

    using Discord.Commands;

    [Export(typeof(IBotCommand))]
    public class ListCommand : BotCommandBase
    {
        private readonly ISoundRepository _soundRepository;

        [ImportingConstructor]
        public ListCommand(ISoundRepository soundRepository)
            : base("list", "List available sounds")
        {
            _soundRepository = soundRepository;
        }

        public override async Task ExecuteAsync(CommandEventArgs args)
        {
            var sounds = _soundRepository.ListSounds();
            
            var builder = new StringBuilder("Available sounds:");
            foreach (var sound in sounds)
            {
                builder.Append("\n  - ");
                builder.Append(sound);
            }

            await SendPrivateMessageAsync(args, builder.ToString());
        }
    }
}
