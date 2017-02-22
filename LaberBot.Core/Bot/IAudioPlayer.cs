namespace LaberBot.Bot
{
    using System.Threading.Tasks;

    using Discord;
    using Discord.Audio;

    public interface IAudioPlayer
    {
        void Init(AudioService audioService);

        Task PlayAsync(Channel channel, string file);

        void Stop();
    }
}