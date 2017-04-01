namespace LaberBot.Bot.Commands
{
    using System.ComponentModel.Composition;
    using System.Threading.Tasks;

    using Discord.Commands;

    [Export(typeof(IBotCommand))]
    public class StopCommand : BotCommandBase
    {
        private readonly IAudioPlayer _player;

        [ImportingConstructor]
        public StopCommand(IAudioPlayer player) : base("stop", "Stop the currently playing sound")
        {
            _player = player;
        }

        public override Task ExecuteAsync(CommandEventArgs args)
        {
            _player.Stop();

            return Task.FromResult<object>(null);
        }
    }
}
