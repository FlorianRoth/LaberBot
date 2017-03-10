namespace LaberBot.Bot
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Linq;
    using System.Threading.Tasks;

    using Discord;
    using Discord.Audio;
    using Discord.Commands;

    using log4net;

    [Export(typeof(ILaberBot))]
    public class LaberBot : ILaberBot
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(LaberBot));

        private readonly DiscordClient _client;
        
        public BotConfiguration Configuration { get; private set; }

        public IReadOnlyCollection<IBotCommand> Commands { get; private set; }

        [ImportingConstructor]
        public LaberBot(
            IAudioPlayer audioPlayer,
            [ImportMany]IEnumerable<IBotCommand> commands)
        {
            Logger.Info("Creating LaberBot");

            _client = new DiscordClient(ConfigureClient);
            _client.UsingCommands(ConfigureCommands);
            _client.UsingAudio(ConfigureAudio);

            audioPlayer.Init(_client.GetService<AudioService>());

            RegisterCommands(commands);
        }
        
        private void ConfigureClient(DiscordConfigBuilder cfg)
        {
            cfg.AppName = "LaberBot";
            cfg.LogLevel = LogSeverity.Verbose;
        }

        private void ConfigureCommands(CommandServiceConfigBuilder cfg)
        {
            cfg.PrefixChar = '!';
            cfg.HelpMode = HelpMode.Public;
        }

        private void ConfigureAudio(AudioServiceConfigBuilder cfg)
        {
            cfg.Mode = AudioMode.Outgoing;
        }

        public void Run(BotConfiguration config)
        {
            Logger.Info("Running LaberBot...");
            Logger.InfoFormat("    Authentication token: {0}", config.AuthToken);
            Logger.InfoFormat("    Sound directory:      {0}", config.SoundDirectory);

            Configuration = config;

            _client.ExecuteAndWait(Connect);

            Logger.Info("LaberBot stopped");
        }

        private async Task Connect()
        {
            await _client.Connect(Configuration.AuthToken, TokenType.Bot);
            
            Logger.Info("Connected to server");

            _client.SetGame("Bortogen");
        }

        private void RegisterCommands(IEnumerable<IBotCommand> commands)
        {
            var commandList = new List<IBotCommand>();
            var commandService = _client.GetService<CommandService>();

            foreach (var cmd in commands)
            {
                Logger.Info($"  Registering command '{cmd.Command}'");

                var builder = commandService.CreateCommand(cmd.Command);
                builder.Description(cmd.Description);
                builder.Alias(cmd.Alias.ToArray());

                foreach (var p in cmd.Parameter)
                {
                    builder.Parameter(p.Item1, p.Item2);
                }

                builder.Do(async args => await ExecuteCommand(cmd, args));

                commandList.Add(cmd);
            }

            Commands = commandList;
        }

        private async Task ExecuteCommand(IBotCommand command, CommandEventArgs args)
        {
            Logger.Info($"Executing command '{command.Command}'");

            try
            {
                await args.Channel.DeleteMessages(new[] { args.Message });
                await command.ExecuteAsync(args);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
            }
        }
    }
}
