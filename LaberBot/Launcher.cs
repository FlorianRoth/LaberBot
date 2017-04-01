namespace LaberBot
{
    using System;
    using System.ComponentModel.Composition;
    using System.ComponentModel.Composition.Hosting;
    using System.IO;
    using System.Reflection;

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

            var botDir = GetBotDirectory();
            
            var assemblyCatalog = new AssemblyCatalog(typeof(Launcher).Assembly);
            var directoryCatalog = new DirectoryCatalog(botDir);

            var catalog = new AggregateCatalog(assemblyCatalog, directoryCatalog);

            var container = new CompositionContainer(catalog);
            container.ComposeExportedValue(container);

            return container;
        }

        private static string GetBotDirectory()
        {
            var codeBase = Assembly.GetExecutingAssembly().CodeBase;
            var uri = new UriBuilder(codeBase);
            var path = Uri.UnescapeDataString(uri.Path);
            return Path.GetDirectoryName(path);
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
