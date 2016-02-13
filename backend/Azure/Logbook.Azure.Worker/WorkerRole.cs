using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace Logbook.Azure.Worker
{
    public class WorkerRole : RoleEntryPoint
    {
        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections
            ServicePointManager.DefaultConnectionLimit = 256;


            return true;
        }

        public override void Run()
        {
     
        }

        public override void OnStop()
        {
            
        }
    }
}
