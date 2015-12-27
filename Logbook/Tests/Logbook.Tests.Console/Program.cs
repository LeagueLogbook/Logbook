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
            string code = "AQA8YJtRc8d-lqDIc5QraGRrzYgCfIbcbqU83hujHnX1PPmZzWeiH-0ZwdK2XLVA5_i0XJ9SnAz2hzzsZaoWHEhkSm2vay1v-Me4497leks3K-gaOQxMdbYjVqkBj_Co6zZjjybobuagHk6EdozXhx-m1vsc9GwWpH8mJgRwqY9cFxD-lS2T_G6vJ5aNXMbK0_yNtOJelvdQzgncYfDA8GH_9h6hW6F-fgVpOidZ_tRdEQvi92ws_H3yyVYZovYQnbRxP6JO3R9RWLETHEUaWVKc3rRbJrY-LJJWTR0Id1A6qH0p4gZh6n8EkEDnrclsmMDM5JZZ0m4hwkTGeH4EtOQgH33KkeGekACpxpZiK-B2AA#_=_";
            string redirectUrl = "https://www.facebook.com/connect/login_success.html";

            var facebookService = new FacebookService();
            var token = facebookService.ExchangeCodeForTokenAsync(redirectUrl, code).Result;
            var me = facebookService.GetMeAsync(token).Result;
        }
    }
}
