namespace LaberBot
{
    using System.ComponentModel.Composition.Hosting;

    using log4net;

    using LaberBot.Bot;
    using LaberBot.Bot.CommandLine;

    public static class Launcher
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(Launcher));

        public static void Run(string[] args)
        {
            var container = SetupContainer();
            EvaluateCommandLine(args, container);

            Run(container);
        }

        private static CompositionContainer SetupContainer()
        {
            Logger.Debug("Initializing composition container");

            var catalog = new AssemblyCatalog(typeof(ILaberBot).Assembly);
            var container = new CompositionContainer(catalog);

            return container;
        }

        private static void EvaluateCommandLine(string[] args, CompositionContainer container)
        {
            Logger.Debug("Evaluating command line");

            var commandLineParser = container.GetExportedValue<ICommandLineParser>();
            commandLineParser.Parse(args);
        }
        
        private static void Run(CompositionContainer container)
        {
            Logger.Debug("Starting bot");

            var bot = container.GetExportedValue<ILaberBot>();
            bot.Run();
        }
    }
}
