namespace LaberBot.Bot
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Discord.Commands;

    public interface IBotCommand
    {
        string Command { get; }

        string Description { get; }

        IEnumerable<string> Alias { get; }

        IReadOnlyList<Tuple<string, ParameterType>> Parameter { get; }
        
        Task ExecuteAsync(CommandEventArgs args);
    }
}
