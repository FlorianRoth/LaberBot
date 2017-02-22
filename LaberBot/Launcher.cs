namespace LaberBot
{
    using System;
    using System.ComponentModel.Composition.Hosting;

    using log4net;

    using LaberBot.Bot;

    public static class Launcher
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(Launcher));

        public static void Run(string[] args)
        {
            var container = SetupContainer();
            var config = CreateConfiguration(args);

            Run(config, container);
        }

        private static CompositionContainer SetupContainer()
        {
            Logger.Debug("Initializing composition container");

            var catalog = new AssemblyCatalog(typeof(ILaberBot).Assembly);
            var container = new CompositionContainer(catalog);

            return container;
        }

        private static BotConfiguration CreateConfiguration(string[] args)
        {
            Logger.Debug("Creating bot configuration");

            if (args.Length != 2)
            {
                Logger.Debug("Creating bot configuration");
                throw new ArgumentException("Invalid number of command line arguments.");
            }

            var config = new BotConfiguration
            {
                AuthToken = args[0],
                SoundDirectory = args[1]
            };

            return config;
        }

        private static void Run(BotConfiguration config, CompositionContainer container)
        {
            Logger.Debug("Starting bot");

            var bot = container.GetExportedValue<ILaberBot>();
            bot.Run(config);
        }
    }
}
