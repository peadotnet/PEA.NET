using System;
using Akka.Actor;
using Pea.ActorModel.Messages;
using Pea.Core;

namespace Pea.ActorModel.Actors
{
    public class FitnessEvaluatorActor : ReceiveActor
    {
        private PeaSettings Settings { get; }

        private IFitnessCalculator Calculator { get; }

        public FitnessEvaluatorActor(PeaSettings settings)
        {
            Settings = settings;
            Calculator = (IFitnessCalculator)Activator.CreateInstance(settings.Fitness);

            Receive<InitFitnessCalculator>(i => InitFitnessCalculator(i));
        }

        private void InitFitnessCalculator(InitFitnessCalculator initMessage)
        {
            Calculator.Init(initMessage.InitData);
        }

        public static Props CreateProps(PeaSettings settings)
        {
            var props = Props.Create(() => new FitnessEvaluatorActor(settings));
            return props;
        }
    }
}
