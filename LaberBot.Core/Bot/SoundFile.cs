namespace LaberBot.Bot
{
    public class SoundFile : ISoundFile
    {
        public string Name { get; }

        public string Path { get; }

        public string Group { get; }

        public SoundFile(string name, string path, string @group)
        {
            Name = name;
            Path = path;
            Group = @group;
        }
    }
}