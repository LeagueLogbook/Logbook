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
            string code = "M935fc443-fd45-c195-d571-f7c9638bfd47";
            string redirectUrl = "https://login.live.com/oauth20_desktop.srf";

            var facebookService = new MicrosoftService();
            var token = facebookService.ExchangeCodeForTokenAsync(redirectUrl, code).Result;
            var me = facebookService.GetMeAsync(token).Result;
        }
    }
}
