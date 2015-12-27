using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logbook.Server.Infrastructure.Social;

namespace Logbook.Tests.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            string code = "4/BP5x5HSAODf9XSj6H1J31TgIT9BOsABhB4pPfDC4GzU#";
            string redirectUrl = "http://localhost/";

            var facebookService = new GoogleService();
            var token = facebookService.ExchangeCodeForTokenAsync(redirectUrl, code).Result;
            var me = facebookService.GetMeAsync(token).Result;
        }
    }
}
