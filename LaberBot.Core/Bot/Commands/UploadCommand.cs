namespace LaberBot.Bot.Commands
{
    using System;
    using System.ComponentModel.Composition;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;

    using Discord.Commands;

    using log4net;

    using Message = Discord.Message;

    //[Export(typeof(IBotCommand))]
    public class UploadCommand : BotCommandBase
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(UploadCommand));

        private readonly ISoundRepository _soundRepository;

        [ImportingConstructor]
        public UploadCommand(ISoundRepository soundRepository)
            : base("upload", "Upload a sound file")
        {
            _soundRepository = soundRepository;
        }

        public override async Task ExecuteAsync(CommandEventArgs args)
        {
            var role = args.Server.FindRoles("Geile Typen", true).FirstOrDefault();
            if (null == role)
            {
                return;
            }

            var user = args.Message.User;
            if (!user.Roles.Contains(role))
            {
                await RespondAsync(args, $"{user.Name}, you have no permission to upload sound files!");
                return;
            }

            foreach (var attachment in args.Message.Attachments)
            {
                await RespondAsync(args, $"Uploading {attachment.Filename}...");
                await DownloadAttachment(args, attachment);
            }
        }

        private async Task DownloadAttachment(CommandEventArgs args, Message.Attachment attachment)
        {
            var upload = _soundRepository.RequestUpload(attachment.Filename);
            if (upload.IsAccepted == false)
            {
                Logger.Warn($"Upload {upload.OriginalName} was not accepted!");
                await RespondAsync(args, $"The file {upload.OriginalName} cannot be uploaded!");
                return;
            }

            var uri = new Uri(attachment.Url);
            Logger.Debug($"Downloading {uri}");
            
            using (var myWebClient = new WebClient())
            {
                try
                {
                    myWebClient.DownloadFile(uri, upload.FilePath);
                    
                    Logger.Debug("Download complete");
                    await RespondAsync(args, $"The file {upload.OriginalName} has been uploaded as '{Path.GetFileNameWithoutExtension(upload.FilePath)}'");
                    await args.Channel.SendTTSMessage("Geil");
                }
                catch (Exception ex)
                {
                    Logger.Error(ex.Message, ex);
                    await RespondAsync(args, $"The file {upload.OriginalName} cannot be uploaded!");
                }
            }
        }
    }
}
