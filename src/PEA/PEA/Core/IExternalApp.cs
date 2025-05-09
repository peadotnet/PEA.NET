using System.Threading.Tasks;

namespace Pea.Core
{
    public interface IExternalApp
    { 
    }

    public interface IExternalApp<TR> : IExternalApp where TR: class
    {
        Task<PeaResult> StartAsync(TR initData);
    }
}
