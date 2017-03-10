namespace LaberBot.Bot.Commands
{
    using System.ComponentModel.Composition;
    using System.Linq;
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
            var soundGroups = _soundRepository.ListSounds().GroupBy(s => s.Group).OrderBy(g => g.Key);
            
            var builder = new StringBuilder("Available sounds:\n");
            foreach (var group in soundGroups)
            {
                builder.Append("\n");
                builder.Append(null != group.Key ? $"Group '{group.Key}'" : "No group");
                
                foreach (var sound in group)
                {
                    builder.Append("\n  - ");
                    builder.Append(sound.Name);
                }
            }

            await SendPrivateMessageAsync(args, builder.ToString());
        }
    }
}
