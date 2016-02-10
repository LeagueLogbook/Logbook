using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logbook.Server.Infrastructure.Emails;
using Logbook.Server.Infrastructure.Emails.Templates;
using Logbook.Server.Infrastructure.Encryption;
using Logbook.Server.Infrastructure.Riot;
using Logbook.Server.Infrastructure.Social;
using Logbook.Shared.Entities.Summoners;

namespace Logbook.Tests.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var leagueService = new LeagueService();

            var game = leagueService.GetCurrentGameAsync(Region.Euw, 24606316).Result;

            System.Console.ReadLine();
        }
    }
}
