using System.Threading.Tasks;
using JetBrains.Annotations;
using Logbook.Shared.Results;

namespace Logbook.Server.Contracts.Commands
{
    public interface ICommandScope
    {
        [NotNull]
        Task<Result<TResult>> Execute<TResult>([NotNull]ICommand<TResult> command);
    }
}