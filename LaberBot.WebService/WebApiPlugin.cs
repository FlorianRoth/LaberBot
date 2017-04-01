namespace LaberBot.WebService
{
    using System.ComponentModel.Composition;
    using System.ComponentModel.Composition.Hosting;
    using System.Web.Http;

    using log4net;

    using LaberBot.Bot;

    using Microsoft.Owin.Hosting;
    using Microsoft.Owin.Logging;

    using Owin;

    [Export(typeof(IBotPlugin))]
    internal class WebApiPlugin : IBotPlugin
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(WebApiPlugin));

        private readonly CompositionContainer _container;

        private readonly WebServiceOptions _options;

        [ImportingConstructor]
        public WebApiPlugin(
            CompositionContainer container,
            WebServiceOptions options)
        {
            _container = container;
            _options = options;
        }

        public void Init(ILaberBot bot)
        {
        }

        public void Run()
        {
            var port = _options.Port;
            var baseAddress = $"http://*:{port}/";

            Logger.InfoFormat("Running Web API server at {0}", baseAddress);
            WebApp.Start(baseAddress, Startup);
        }

        private void Startup(IAppBuilder appBuilder)
        {
            var config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();
            config.DependencyResolver = new MefDependencyResolver(_container);

            appBuilder.SetLoggerFactory(new LoggerFactory());
            appBuilder.UseWebApi(config);
        }
    }
}
