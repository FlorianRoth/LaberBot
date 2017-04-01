namespace LaberBot.Bot
{
    public interface ISoundFile
    {
        string Name { get; }

        string Path { get; }

        string Group { get; }
    }
}