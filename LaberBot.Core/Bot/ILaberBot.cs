namespace LaberBot.Bot
{
    using System.Collections.Generic;

    public interface ILaberBot
    {
        BotConfiguration Configuration { get; }

        IReadOnlyCollection<IBotCommand> Commands { get; }

        void Run(BotConfiguration config);
    }
}