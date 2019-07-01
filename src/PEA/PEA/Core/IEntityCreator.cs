namespace Pea.Core
{
    public interface IEntityCreator
    {
        void Init(IEvaluationInitData initData);
        IEntity CreateEntity();
    }
}
