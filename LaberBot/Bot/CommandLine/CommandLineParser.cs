namespace LaberBot.Bot.CommandLine
{
    using System.Collections.Generic;
    using System.ComponentModel.Composition;

    using global::CommandLine;

    [Export(typeof(ICommandLineParser))]
    public class CommandLineParser : ICommandLineParser
    {
        private readonly IEnumerable<ICommandLineOptions> _optionsList;

        [ImportingConstructor]
        public CommandLineParser([ImportMany] IEnumerable<ICommandLineOptions> optionsList)
        {
            _optionsList = optionsList;
        }

        public void Parse(string[] args)
        {
            var parser = new Parser(ConfigureCommandLineParser);

            foreach (var options in _optionsList)
            {
                parser.ParseArguments(args, options);
            }
        }

        private static void ConfigureCommandLineParser(ParserSettings config)
        {
            config.IgnoreUnknownArguments = true;
            config.CaseSensitive = false;
        }
    }
}
