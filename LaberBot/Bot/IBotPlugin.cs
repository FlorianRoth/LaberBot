namespace LaberBot.Bot
{
    public interface IBotPlugin
    {
        void Init(ILaberBot bot);

        void Run();
    }
}
