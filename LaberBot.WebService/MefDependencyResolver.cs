namespace LaberBot.WebService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.ComponentModel.Composition.Hosting;
    using System.Linq;
    using System.Web.Http.Dependencies;

    [Export]
    public class MefDependencyResolver : IDependencyResolver
    {
        private CompositionContainer _container;

        [ImportingConstructor]
        public MefDependencyResolver(CompositionContainer container)
        {
            _container = container;
        }

        public void Dispose()
        {
            _container = null;
        }

        public object GetService(Type serviceType)
        {
            return _container.GetExports(serviceType, null, null).Select(l => l.Value).FirstOrDefault();
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _container.GetExports(serviceType, null, null).Select(l => l.Value);
        }

        public IDependencyScope BeginScope()
        {
            var container = new CompositionContainer(_container);
            var scope = new MefDependencyResolver(container);
            return scope;
        }
    }
}
