using System.Threading.Tasks;
using JetBrains.Annotations;
using Logbook.Server.Contracts.Commands;

namespace Logbook.Server.Infrastructure.Extensions
{
    public static class CommandExecutorExtensions
    {
        public static Task<TResult> Execute<TResult>(this ICommandExecutor commandExecutor, ICommand<TResult> command)
        {
            return commandExecutor.Batch(f => f.Execute(command));
        }
    }
}