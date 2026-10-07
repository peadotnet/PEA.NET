using Pea.Core;
using Pea.Core.Entity;

namespace Pea.Tests.AlgorithmTests
{
    public class TestEvaluation : EvaluationBase
    {
        public TestEvaluation(ParameterSet parameters) : base(parameters)
        {
        }

        public override IList<EntityBase> Combine(MultiKey islandKey, Dictionary<MultiKey, EntityBase> entities)
        {
            return new List<EntityBase>();
        }

        public override EntityBase Decode(MultiKey islandKey, Dictionary<MultiKey, EntityBase> entities)
        {
            return new TestEntity(1);
        }

        public override void Init(IEvaluationInitData initData)
        {
        }
    }
}
