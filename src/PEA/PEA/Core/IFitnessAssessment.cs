namespace Pea.Core
{
    public interface IFitnessAssessment
    {
        void Init(IFitnessAssessmentInitData initData);
        IEntity AssessFitness(IEntity entity);
    }
}
