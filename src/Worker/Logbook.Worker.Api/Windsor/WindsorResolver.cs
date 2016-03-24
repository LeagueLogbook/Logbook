using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Dependencies;
using Castle.Windsor;

namespace Logbook.Worker.Api.Windsor
{
    public class WindsorResolver : IDependencyResolver
    {
        #region Fields
        private readonly IWindsorContainer _container;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="WindsorResolver"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public WindsorResolver(IWindsorContainer container)
        {
            this._container = container;
        }
        #endregion

        #region Implementation of IDependencyScope
        /// <summary>
        /// Starts a resolution scope.
        /// </summary>
        public IDependencyScope BeginScope()
        {
            return new WindsorDependencyScope(this._container);
        }
        /// <summary>
        /// Retrieves a service from the scope.
        /// </summary>
        /// <param name="serviceType">The service to be retrieved.</param>
        public object GetService(Type serviceType)
        {
            if (this._container.Kernel.HasComponent(serviceType) == false)
                return null;

            return this._container.Resolve(serviceType);
        }
        /// <summary>
        /// Retrieves a collection of services from the scope.
        /// </summary>
        /// <param name="serviceType">The collection of services to be retrieved.</param>
        public IEnumerable<object> GetServices(Type serviceType)
        {
            if (this._container.Kernel.HasComponent(serviceType) == false)
                return new object[0];

            return this._container.ResolveAll(serviceType).Cast<object>();
        }
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this._container.Dispose();
        }
        #endregion
    }
}