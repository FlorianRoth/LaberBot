namespace LaberBot.Bot
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.IO;
    using System.Linq;

    [Export(typeof(ISoundRepository))]
    public class SoundRepository : ISoundRepository
    {
        private const string SUFFIX = ".wav";

        private readonly Lazy<ILaberBot> _bot;

        private string SoundDirectory => _bot.Value.Configuration.SoundDirectory;

        [ImportingConstructor]
        public SoundRepository(Lazy<ILaberBot> bot)
        {
            _bot = bot;
        }

        public IEnumerable<string> ListSounds()
        {
            return Directory
                .EnumerateFiles(SoundDirectory, $"*{SUFFIX}", SearchOption.TopDirectoryOnly)
                .Select(Path.GetFileNameWithoutExtension);
        }

        public string GetSoundFile(string name)
        {
            var file = Path.Combine(SoundDirectory, $"{name}{SUFFIX}");

            return File.Exists(file) ? file : null;
        }
        
        public UploadRequest RequestUpload(string filename)
        {
            var isAccepted = filename.EndsWith(SUFFIX);

            var path = GetFilePath(filename);
            
            return new UploadRequest(filename, path, isAccepted);
        }

        private string GetFilePath(string filename)
        {
            var path = Path.Combine(SoundDirectory, filename);

            if (!File.Exists(path))
            {
                return path;
            }

            var nameWithoutExtension = Path.GetFileNameWithoutExtension(filename);

            var possibleNames = Enumerable.Range(0, int.MaxValue).Select(i => CreateFileName(nameWithoutExtension, i));
            var name = possibleNames.First(f => File.Exists(f) == false);

            return name;
        }

        private string CreateFileName(string nameWithoutExtension, int counter)
        {
            return Path.Combine(SoundDirectory, $"{nameWithoutExtension}{counter}{SUFFIX}");
        }
    }
}
