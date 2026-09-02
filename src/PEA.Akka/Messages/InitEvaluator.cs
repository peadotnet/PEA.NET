using Pea.Core;

namespace PEA.Akka.Messages
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
