namespace LaberBot.Bot
{
    using Discord;

    internal class DiscordUser : IUser
    {
        private readonly User _user;

        public string Name => _user.Name;

        public IChannel VoiceChannel => null == _user.VoiceChannel ? null : new DiscordChannel(_user.VoiceChannel);

        public DiscordUser(User user)
        {
            _user = user;
        }
    }
}
