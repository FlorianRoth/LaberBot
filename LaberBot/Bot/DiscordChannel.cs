namespace LaberBot.Bot
{
    using Discord;

    internal class DiscordChannel : IChannel
    {
        public Channel Channel { get; }

        public string Name => Channel.Name;

        public DiscordChannel(Channel channel)
        {
            Channel = channel;
        }
    }
}
