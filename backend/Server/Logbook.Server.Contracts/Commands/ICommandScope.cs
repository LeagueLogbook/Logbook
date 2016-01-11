﻿using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Logbook.Server.Contracts.Commands
{
    public interface ICommandScope
    {
        [NotNull]
        Task<TResult> Execute<TResult>([NotNull]ICommand<TResult> command);
    }
}