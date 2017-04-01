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

    using CommandLine;

    using log4net;

    [Export(typeof(ILaberBot))]
    public class LaberBot : ILaberBot
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(LaberBot));

        private readonly DefaultOptions _options;
        
        public DiscordClient Client { get; }

        public IReadOnlyCollection<IBotPlugin> Plugins { get; private set; }

        public IReadOnlyCollection<IBotCommand> Commands { get; private set; }

        [ImportingConstructor]
        public LaberBot(
            DefaultOptions options,
            [ImportMany]IEnumerable<IBotPlugin> plugins,
            [ImportMany]IEnumerable<IBotCommand> commands)
        {
            _options = options;
            Logger.Info("Creating LaberBot");

            Client = new DiscordClient(ConfigureClient);
            Client.UsingCommands(ConfigureCommands);
            Client.UsingAudio(ConfigureAudio);

            InitPlugins(plugins);

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

        public void Run()
        {
            Logger.Info("Running LaberBot...");
            Logger.InfoFormat("    Authentication token: {0}", _options.AuthToken);
            Logger.InfoFormat("    Sound directory:      {0}", _options.SoundDirectory);
            
            Client.ExecuteAndWait(Connect);

            Logger.Info("LaberBot stopped");
        }

        private async Task Connect()
        {
            await Client.Connect(_options.AuthToken, TokenType.Bot);

            Logger.Info("Connected to server");

            Client.SetGame(_options.Game);

            foreach (var plugin in Plugins)
            {
                plugin.Run();
            }
        }

        private void InitPlugins(IEnumerable<IBotPlugin> plugins)
        {
            Logger.Info("Loading plugins:");

            Plugins = plugins.ToList();

            foreach (var plugin in Plugins)
            {
                Logger.InfoFormat("    - {0}", plugin.GetType().Name);
                plugin.Init(this);
            }
        }

        private void RegisterCommands(IEnumerable<IBotCommand> commands)
        {
            var commandList = new List<IBotCommand>();
            var commandService = Client.GetService<CommandService>();
            
            Logger.Info("Registering commands:");

            foreach (var cmd in commands)
            {
                Logger.Info($"  - {cmd.Command}");

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
