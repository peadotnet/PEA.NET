using System.Threading.Tasks;

namespace Pea
{
    public interface IExternalApp
    { 
    }

    public interface IExternalApp<TR> : IExternalApp where TR: class
    {
        Task<PeaResult> StartAsync(TR initData);
    }
}
