using System.Threading.Tasks;
using JetBrains.Annotations;
using Logbook.Server.Contracts.Commands;
using Logbook.Shared.Results;

namespace Logbook.Server.Infrastructure.Extensions
{
    public static class CommandExecutorExtensions
    {
        [NotNull]
        public static Task<Result<TResult>> Execute<TResult>([NotNull]this ICommandExecutor commandExecutor, [NotNull]ICommand<TResult> command)
        {
            return commandExecutor.Batch(f => f.Execute(command));
        }
    }
}