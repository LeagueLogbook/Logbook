using System.Threading.Tasks;

namespace Logbook.Server.Contracts.Commands
{
    public interface ICommandScope
    {
        Task<TResult> Execute<TResult>(ICommand<TResult> command);
        
        Task<TTarget> Map<TSource, TTarget>(TSource source);
    }
}