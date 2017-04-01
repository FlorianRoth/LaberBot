namespace LaberBot.WebService
{
    using System.ComponentModel.Composition;

    using CommandLine;

    using LaberBot.Bot.CommandLine;

    [Export(typeof(WebServiceOptions))]
    [Export(typeof(ICommandLineOptions))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class WebServiceOptions : ICommandLineOptions
    {
        [Option('p', "port", DefaultValue = 9000, HelpText = "The port of the Web API server")]
        public int Port { get; set; }

        [Option('s', "server", Required = true)]
        public string Server { get; set; }
    }
}
