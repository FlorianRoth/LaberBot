namespace LaberBot.Bot.CommandLine
{
    using System.ComponentModel.Composition;

    using global::CommandLine;

    [Export(typeof(DefaultOptions))]
    [Export(typeof(ICommandLineOptions))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class DefaultOptions : ICommandLineOptions
    {
        [Option('t', "token", Required = true, HelpText = "The authentication token")]
        public string AuthToken { get; set; }

        [Option('d', "sounds", Required = true, HelpText = "The directory containing the sound files")]
        public string SoundDirectory { get; set; }

        [Option('g', "game", Required = false, HelpText = "The game name to display", DefaultValue = "Bortogen")]
        public string Game { get; set; }
    }
}
