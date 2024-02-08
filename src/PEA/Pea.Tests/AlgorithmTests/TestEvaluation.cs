using Pea.Core;

namespace Pea.Tests.AlgorithmTests
{
    public class TestEvaluation : EvaluationBase
    {
        public TestEvaluation(ParameterSet parameters) : base(parameters)
        {
        }

        public override IList<IEntity> Combine(MultiKey islandKey, Dictionary<MultiKey, IEntity> entities)
        {
            return new List<IEntity>();
        }

        public override IEntity Decode(MultiKey islandKey, Dictionary<MultiKey, IEntity> entities)
        {
            return new TestEntity(1);
        }

        public override void Init(IEvaluationInitData initData)
        {
        }
    }
}
