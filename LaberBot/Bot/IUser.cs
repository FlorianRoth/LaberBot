namespace LaberBot.Bot
{
    public interface IUser
    {
        string Name { get; }

        IChannel VoiceChannel { get; }
    }
}
