using Pea.Core.Entity;

namespace Pea.Core
{
    public interface IFitnessAssessment
    {
        void Init(IFitnessAssessmentInitData initData);
        EntityBase AssessFitness(EntityBase entity);
    }
}
