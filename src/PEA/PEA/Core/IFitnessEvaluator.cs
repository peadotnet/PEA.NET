using System.Collections.Generic;

namespace Pea.Core
{
    public interface IFitnessEvaluator
    {
        IList<IEntity> AssessFitness(IList<IEntity> entityList);
    }
}
