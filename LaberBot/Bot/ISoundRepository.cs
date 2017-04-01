namespace LaberBot.Bot
{
    using System.Collections.Generic;

    public interface ISoundRepository
    {
        IEnumerable<ISoundFile> ListSounds();

        ISoundFile GetSoundFile(string name);

        UploadRequest RequestUpload(string filename);
    }
}