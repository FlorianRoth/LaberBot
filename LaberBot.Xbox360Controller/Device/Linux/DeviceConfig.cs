namespace LaberBot.Xbox360Controller.Device.Linux
{
    using System.ComponentModel.Composition;

    using CommandLine;

    using LaberBot.Bot.CommandLine;

    [Export(typeof(DeviceConfig))]
    [Export(typeof(ICommandLineOptions))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class DeviceConfig : ICommandLineOptions
    {
        [Option('i', "inputDevice", Required = true, HelpText = "The input device")]
        public string Device { get; set; }

    }
}
