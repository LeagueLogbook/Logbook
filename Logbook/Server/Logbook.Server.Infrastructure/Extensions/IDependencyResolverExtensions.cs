using System.Web.Http.Dependencies;

namespace Logbook.Server.Infrastructure.Extensions
{
    public static class IDependencyResolverExtensions 
    {
        public static TService GetService<TService>(this IDependencyResolver resolver)
        {
            return (TService)resolver.GetService(typeof (TService));
        }
    }
}