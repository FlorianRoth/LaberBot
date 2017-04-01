namespace LaberBot.Bot
{
    using System.Collections.Generic;

    public interface IServer
    {
        string Name { get; }

        IEnumerable<IUser> FindUsers(string name);
    }
}
