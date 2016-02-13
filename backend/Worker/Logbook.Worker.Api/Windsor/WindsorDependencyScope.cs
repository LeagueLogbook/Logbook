using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Dependencies;
using Castle.MicroKernel.Lifestyle;
using Castle.Windsor;

namespace Logbook.Worker.Api.Windsor
{
    public class WindsorDependencyScope : IDependencyScope
    {
        #region Fields
        private readonly IWindsorContainer _container;
        private readonly IDisposable _scope;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="WindsorDependencyScope"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public WindsorDependencyScope(IWindsorContainer container)
        {
            this._container = container;
            this._scope = container.BeginScope();
        }
        #endregion

        #region Implementation of IDependencyScope
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
        #endregion

        #region Implementation of IDisposable
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this._scope.Dispose();
        }
        #endregion
    }
}