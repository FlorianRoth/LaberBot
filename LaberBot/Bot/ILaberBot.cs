namespace LaberBot.Bot
{
    using System.Collections.Generic;

    using Discord;

    public interface ILaberBot
    {
        IReadOnlyCollection<IBotCommand> Commands { get; }
        
        DiscordClient Client { get; }

        void Run();
    }
}