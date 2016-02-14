using System.Web.Http.Dependencies;
using Logbook.Shared;

namespace Logbook.Worker.Api.Extensions
{
    public static class DependencyResolverExtensions 
    {
        public static TService GetService<TService>(this IDependencyResolver self)
        {
            Guard.NotNull(self, nameof(self));

            return (TService)self.GetService(typeof (TService));
        }
    }
}