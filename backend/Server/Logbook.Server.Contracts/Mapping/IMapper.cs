using System.Threading.Tasks;

namespace Logbook.Server.Contracts.Mapping
{
    public interface IMapper<in TSource, TTarget>
    {
        Task<TTarget> MapAsync(TSource source);
    }
}