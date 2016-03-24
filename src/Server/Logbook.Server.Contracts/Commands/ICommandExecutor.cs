using System;
using System.Threading.Tasks;

namespace Logbook.Server.Contracts.Commands
{
    public interface ICommandExecutor
    {
        Task<T> Batch<T>(Func<ICommandScope, Task<T>> batchAction);
    }
}