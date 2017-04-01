namespace LaberBot.Bot
{
    using System;
    using System.ComponentModel.Composition;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;

    using Discord;
    using Discord.Audio;

    using log4net;

    [Export(typeof(IAudioPlayer))]
    public class AudioPlayer : IAudioPlayer
    {
        private class PlayRequest
        {
            public string File { get; }
            
            public CancellationTokenSource TokenSource { get; }
            
            public PlayRequest(string file)
            {
                File = file;
                TokenSource = new CancellationTokenSource();
            }
        }


        private const int BLOCK_SIZE = 3840;


        private static readonly ILog Logger = LogManager.GetLogger(typeof(AudioPlayer));

        private readonly object _lock;


        private AudioService _audioService;

        private PlayRequest _currentRequest;

        public AudioPlayer()
        {
            _lock = new object();
        }
        
        public void Init(AudioService audioService)
        {
            _audioService = audioService;
        }

        public async Task PlayAsync(Channel channel, string file)
        {
            var request = new PlayRequest(file);

            lock (_lock)
            {
                if (null != _currentRequest)
                {
                    Logger.Warn("Already playing an audio file");
                    return;
                }

                _currentRequest = request;
            }
            
            try
            {
                if (null == channel)
                {
                    Logger.Error("Voice channel not found");
                    return;
                }

                Logger.Debug($"Joining channel: {channel.Name}");
                var client = await _audioService.Join(channel);

                Logger.Debug($"Playing audio file: {file}");
                SendAudioData(client, request);

                Logger.Debug($"Leaving channel: {channel.Name}");
                await _audioService.Leave(channel);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
            }

            lock (_lock)
            {
                _currentRequest = null;
            }
        }

        public void Stop()
        {
            lock (_lock)
            {
                if (null == _currentRequest)
                {
                    Logger.Debug("Cannot stop because no audio is currently played.");
                    return;
                }
                
                try
                {
                    _currentRequest.TokenSource.Cancel();
                }
                catch (Exception ex)
                {
                    Logger.Error(ex.Message, ex);
                }
            }
        }

        private void SendAudioData(IAudioClient audioClient, PlayRequest request)
        {
            try
            {
                var file = request.File;
                var cancellationToken = request.TokenSource.Token;

                using (var reader = File.OpenRead(file))
                {
                    byte[] buffer = new byte[BLOCK_SIZE];
                    int byteCount;

                    while ((byteCount = reader.Read(buffer, 0, BLOCK_SIZE)) > 0)
                    {
                        if (cancellationToken.IsCancellationRequested)
                        {
                            audioClient.Clear();
                            return;
                        }
                        
                        audioClient.Send(buffer, 0, byteCount);
                    }
                }

                audioClient.Wait();
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
            }
        }
    }
}
