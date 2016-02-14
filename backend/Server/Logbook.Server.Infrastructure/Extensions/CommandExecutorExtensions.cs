using System.Threading.Tasks;
using JetBrains.Annotations;
using Logbook.Server.Contracts.Commands;
using Logbook.Shared;

namespace Logbook.Server.Infrastructure.Extensions
{
    public static class CommandExecutorExtensions
    {
        public static Task<TResult> Execute<TResult>(this ICommandExecutor commandExecutor, ICommand<TResult> command)
        {
            Guard.NotNull(commandExecutor, nameof(commandExecutor));
            Guard.NotNull(command, nameof(command));

            return commandExecutor.Batch(f => f.Execute(command));
        }
    }
}