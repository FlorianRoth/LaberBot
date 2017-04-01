namespace LaberBot.WebService
{
    using System;
    using System.Diagnostics;

    using log4net;

    using Microsoft.Owin.Logging;

    internal class Logger : ILogger
    {
        private readonly ILog _logger;

        public Logger(string name)
        {
            _logger = LogManager.GetLogger(name);
        }

        public bool WriteCore(TraceEventType eventType, int eventId, object state, Exception exception, Func<object, Exception, string> formatter)
        {
            switch (eventType)
            {
                case TraceEventType.Critical:
                    _logger.Fatal(formatter(state, exception), exception);
                    break;

                case TraceEventType.Error:
                    _logger.Error(formatter(state, exception), exception);
                    break;

                case TraceEventType.Warning:
                    _logger.Warn(formatter(state, exception), exception);
                    break;

                case TraceEventType.Information:
                    _logger.Info(formatter(state, exception), exception);
                    break;

                case TraceEventType.Verbose:
                    _logger.Debug(formatter(state, exception), exception);
                    break;

                default:
                    _logger.Debug(formatter(state, exception), exception);
                    break;
            }

            return true;
        }
    }
}