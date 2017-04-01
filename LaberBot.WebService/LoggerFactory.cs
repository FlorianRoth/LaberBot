namespace LaberBot.WebService
{
    using System.Collections.Generic;

    using Microsoft.Owin.Logging;

    internal class LoggerFactory : ILoggerFactory
    {
        private readonly IDictionary<string, ILogger> _loggers;

        public LoggerFactory()
        {
            _loggers = new Dictionary<string, ILogger>();
        }
        
        public ILogger Create(string name)
        {
            ILogger logger;
            if (false == _loggers.TryGetValue(name, out logger))
            {
                logger = new Logger(name);
                _loggers[name] = logger;
            }

            return logger;
        }
    }
}