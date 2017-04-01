namespace LaberBot.Bot
{
    using System.Threading.Tasks;
    
    public interface IAudioPlayer
    {
        Task PlayAsync(IChannel channel, string file);

        void Stop();
    }
}