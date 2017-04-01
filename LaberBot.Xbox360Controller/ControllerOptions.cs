namespace LaberBot.Xbox360Controller
{
    using System.ComponentModel.Composition;

    using CommandLine;

    using LaberBot.Bot.CommandLine;

    [Export(typeof(ControllerOptions))]
    [Export(typeof(ICommandLineOptions))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class ControllerOptions : ICommandLineOptions
    {
        [Option('s', "server", Required = true)]
        public string Server { get; set; }

        [Option('u', "user", Required = true)]
        public string User { get; set; }
    }
}
