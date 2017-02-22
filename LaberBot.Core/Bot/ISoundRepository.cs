namespace LaberBot.Bot
{
    using System.Collections.Generic;

    public class UploadRequest
    {
        public string OriginalName { get; }

        public string FilePath { get; }

        public bool IsAccepted { get; }

        public UploadRequest(string originalName, string filePath, bool isAccepted)
        {
            OriginalName = originalName;
            FilePath = filePath;
            IsAccepted = isAccepted;
        }
    }

    public interface ISoundRepository
    {
        IEnumerable<string> ListSounds();

        string GetSoundFile(string name);

        UploadRequest RequestUpload(string filename);
    }
}