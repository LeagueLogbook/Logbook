using System.Threading.Tasks;
using JetBrains.Annotations;
using Logbook.Server.Contracts.Commands;

namespace Logbook.Server.Infrastructure.Commands
{
    public interface ICommandHandler<in TCommand, TResult>
        where TCommand : ICommand<TResult>
    {
        /// <summary>
        /// Executes the specified <paramref name="command"/>.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="scope">The scope.</param>
        Task<TResult> Execute(TCommand command, ICommandScope scope);
    }
}