namespace LaberBot.Bot.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Discord.Commands;

    public abstract class BotCommandBase : IBotCommand
    {
        public string Command { get; }

        public string Description { get; }

        public IEnumerable<string> Alias { get; protected set; }

        public IReadOnlyList<Tuple<string, ParameterType>> Parameter { get; protected set; }
        
        protected BotCommandBase(string command,  string desc)
        {
            Command = command;
            Description = desc;
            Alias = Enumerable.Empty<string>();
            Parameter = new Tuple<string, ParameterType>[0];
        }
        
        public abstract Task ExecuteAsync(CommandEventArgs args);
        
        protected Task SendMessageToChannelAsync(CommandEventArgs args, string message)
        {
            return args.Channel.SendMessage(message);
        }

        protected Task SendPrivateMessageAsync(CommandEventArgs args, string message)
        {
            return args.User.SendMessage(message);
        }
    }
}
