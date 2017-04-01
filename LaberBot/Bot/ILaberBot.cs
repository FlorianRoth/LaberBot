namespace LaberBot.Bot
{
    using System.Collections.Generic;
    
    public interface ILaberBot
    {
        IReadOnlyCollection<IBotCommand> Commands { get; }
        
        void Run();

        IEnumerable<IServer> FindServers(string name);

        T GetService<T>() where T : class;
    }
}