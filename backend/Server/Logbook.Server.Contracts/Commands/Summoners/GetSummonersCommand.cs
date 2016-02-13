﻿using System.Collections.Generic;
using Logbook.Shared;
using Logbook.Shared.Entities.Summoners;

namespace Logbook.Server.Contracts.Commands.Summoners
{
    public class GetSummonersCommand : ICommand<IList<Summoner>>
    {
        public GetSummonersCommand(int userId)
        {
            Guard.NotZeroOrNegative(userId, nameof(userId));

            this.UserId = userId;
        }

        public int UserId { get; }
    }
}