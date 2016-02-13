using System.Web.Http.Dependencies;

namespace Logbook.Worker.Api.Extensions
{
    public static class IDependencyResolverExtensions 
    {
        public static TService GetService<TService>(this IDependencyResolver resolver)
        {
            return (TService)resolver.GetService(typeof (TService));
        }
    }
}