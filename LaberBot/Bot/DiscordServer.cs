namespace LaberBot.Bot
{
    using System.Collections.Generic;
    using System.Linq;

    using Discord;

    internal class DiscordServer : IServer
    {
        private readonly Server _server;

        public string Name => _server.Name;

        public DiscordServer(Server server)
        {
            _server = server;
        }

        public IEnumerable<IUser> FindUsers(string name)
        {
            return _server.FindUsers(name, exactMatch: true).Select(u => new DiscordUser(u));
        }
    }
}
