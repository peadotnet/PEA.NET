using Pea.Core;

namespace Pea.ActorModel.Messages
{
    public class InitEvaluator
    {
        public IEvaluationInitData InitData { get; }

        public InitEvaluator(IEvaluationInitData initdata)
        {
            InitData = initdata;
        }
    }
}
