using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logbook.Server.Infrastructure.Emails;
using Logbook.Server.Infrastructure.Emails.Templates;
using Logbook.Server.Infrastructure.Encryption;
using Logbook.Server.Infrastructure.Social;

namespace Logbook.Tests.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var twitterService = new TwitterService(new EncryptionService());

            var url = twitterService.GetLoginUrlAsync("http://localhost/twitter-redirect").Result;
            Process.Start(url.Url);

            string verifier = System.Console.ReadLine();

            var token = twitterService.ExchangeForToken(url.Payload, verifier).Result;
            var me = twitterService.GetMeAsync(token).Result;

            System.Console.WriteLine(me.Id);
            System.Console.WriteLine(me.Locale);
            System.Console.WriteLine(me.Email);

            System.Console.ReadLine();
        }
    }
}
