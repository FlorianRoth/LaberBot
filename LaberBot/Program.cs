namespace LaberBot
{
    using System;

    using log4net;
    using log4net.Config;
    
    /// <summary>
    /// Authorize with 
    /// https://discordapp.com/api/oauth2/authorize?client_id=282241767523483649&scope=bot&permissions=3161088
    /// </summary>
    public class Program
    {
        private const int EXIT_SUCCESS = 0;
        private const int EXIT_USAGE = 1;
        private const int EXIT_EXCEPTION = 2;

        public static int Main(string[] args)
        {
            InitLogging();

            try
            {
                Launcher.Run(args);
                return EXIT_SUCCESS;
            }
            catch (ArgumentException ex)
            {
                LogManager.GetLogger(typeof(Program)).Debug("Invalid command line arguments", ex);
                PrintUsage();
                return EXIT_USAGE;
            }
            catch (Exception ex)
            {
                LogManager.GetLogger(typeof(Program)).Error("Failed to execute LaberBot", ex);
                return EXIT_EXCEPTION;
            }
        }

        private static void PrintUsage()
        {
            Console.WriteLine("Usage: LaberBot.exe <authToken> <sound folder>");
        }

        private static void InitLogging()
        {
            XmlConfigurator.Configure();
        }
    }
}
