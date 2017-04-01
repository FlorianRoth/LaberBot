namespace LaberBot.Bot
{
    using System.Collections.Generic;

    public interface ILaberBot
    {
        IReadOnlyCollection<IBotCommand> Commands { get; }

        void Run();
    }
}