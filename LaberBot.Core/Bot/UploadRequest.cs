namespace LaberBot.Bot
{
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
}
