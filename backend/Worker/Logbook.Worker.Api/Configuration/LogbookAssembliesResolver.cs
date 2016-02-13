using System.Collections.Generic;
using System.Reflection;
using System.Web.Http.Dispatcher;
using Logbook.Worker.Api.Controllers;

namespace Logbook.Worker.Api.Configuration
{
    public class LogbookAssembliesResolver : DefaultAssembliesResolver
    {
        /// <summary>
        /// Gets the assemblies.
        /// </summary>
        public override ICollection<Assembly> GetAssemblies()
        {
            return new List<Assembly>
            {
                typeof(BaseController).Assembly
            };
        }
    }
}