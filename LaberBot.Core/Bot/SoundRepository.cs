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

        private readonly IDictionary<string, ISoundFile> _soundFiles;

        private readonly FileSystemWatcher _watcher;

        private string SoundDirectory => _bot.Value.Configuration.SoundDirectory;

        [ImportingConstructor]
        public SoundRepository(Lazy<ILaberBot> bot)
        {
            _bot = bot;
            _soundFiles = new Dictionary<string, ISoundFile>();

            _watcher = new FileSystemWatcher();
            _watcher.Created += OnFileCreated;
            _watcher.Deleted += OnFileDeleted;
            _watcher.Renamed += OnFileRenamed;
        }
        
        private void EnsureInitialized()
        {
            if (_bot.IsValueCreated)
            {
                return;
            }

            LoadSoundFiles();
            StartWatching();
        }
        
        private void StartWatching()
        {
            _watcher.IncludeSubdirectories = true;
            _watcher.Path = SoundDirectory;
            _watcher.Filter = $"*{SUFFIX}";
            _watcher.EnableRaisingEvents = true;
        }

        public IEnumerable<ISoundFile> ListSounds()
        {
            EnsureInitialized();

            return _soundFiles.Values;
        }

        public ISoundFile GetSoundFile(string name)
        {
            EnsureInitialized();

            ISoundFile file;
            _soundFiles.TryGetValue(name, out file);

            return file;
        }

        public UploadRequest RequestUpload(string filename)
        {
            EnsureInitialized();

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


        private void LoadSoundFiles()
        {
            var soundFiles = Directory
                .EnumerateFiles(SoundDirectory, $"*{SUFFIX}", SearchOption.AllDirectories)
                .Select(CreateSoundFile);

            foreach (var file in soundFiles)
            {
                AddSoundFile(file);
            }
        }

        private ISoundFile CreateSoundFile(string path)
        {
            var name = Path.GetFileNameWithoutExtension(path);
            var group = GetGroup(path);
            return new SoundFile(name, path, group);
        }

        private string GetGroup(string path)
        {
            var dir = Path.GetDirectoryName(path);

            if (dir == SoundDirectory)
            {
                return null;
            }

            return Path.GetFileName(dir);
        }

        private void OnFileCreated(object sender, FileSystemEventArgs args)
        {
            var file = CreateSoundFile(args.FullPath);
            AddSoundFile(file);
        }

        private void OnFileDeleted(object sender, FileSystemEventArgs args)
        {
            var file = CreateSoundFile(args.FullPath);
            RemoveSoundFile(file);
        }

        private void OnFileRenamed(object sender, RenamedEventArgs args)
        {
            var oldFile = CreateSoundFile(args.OldFullPath);
            var newFile = CreateSoundFile(args.FullPath);

            RemoveSoundFile(oldFile);
            AddSoundFile(newFile);
        }

        private void AddSoundFile(ISoundFile file)
        {
            _soundFiles[file.Name] = file;
        }

        private void RemoveSoundFile(ISoundFile file)
        {
            _soundFiles.Remove(file.Name);
        }
    }
}
