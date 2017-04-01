namespace LaberBot.Bot
{
    using System.Threading.Tasks;

    using Discord;

    public interface IAudioPlayer
    {
        Task PlayAsync(Channel channel, string file);

        void Stop();
    }
}