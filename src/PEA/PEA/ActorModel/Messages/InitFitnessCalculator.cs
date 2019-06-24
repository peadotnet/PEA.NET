using Pea.Core;

namespace Pea.ActorModel.Messages
{
    public class InitFitnessCalculator
    {
        public IFitnessCalculatorInitData InitData { get; }

        public InitFitnessCalculator(IFitnessCalculatorInitData initdata)
        {
            InitData = initdata;
        }
    }
}
