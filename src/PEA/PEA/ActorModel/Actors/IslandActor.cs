using System;
using System.Collections.Generic;
using Akka.Actor;
using Pea.ActorModel.Messages;
using Pea.Core;
using Pea.Island;

namespace Pea.ActorModel.Actors
{
    public class IslandActor : ReceiveActor
    {
        private IEngine Engine { get; }

        private IAlgorithm Algorithm { get; }

        private IActorRef FitnessEvaluator { get; set; }

        private IActorRef PhenotypeDecoder { get; set; }

        public IslandActor(PeaSettings settings)
        {
            Engine = IslandEngineFactory.Create(settings);

            var algorithmFactory = (IAlgorithmFactory)Activator.CreateInstance(settings.Algorithm);
            Algorithm = algorithmFactory.GetAlgorithm(null, Engine, DecodePhenotype, AssessFitness);

            Receive<Continue>(m => RunOneStep());
        }

        public void RunOneStep()
        {
            bool stop = Engine.RunOnce();
            if (!stop)
            {
                Self.Tell(Continue.Instance);
            }
        }

        public IList<IEntity> DecodePhenotype(IList<IEntity> entityList)
        {
            var decodedEntities = PhenotypeDecoder.Ask(entityList).GetAwaiter().GetResult() as IList<IEntity>;
            return decodedEntities;
        }

        public IList<IEntity> AssessFitness(IList<IEntity> entityList)
        {
            var assessedEntities = FitnessEvaluator.Ask(entityList).GetAwaiter().GetResult() as IList<IEntity>;
            return assessedEntities;
        }

        public static Props CreateProps(PeaSettings settings)
        {
            var props = Props.Create(() => new IslandActor(settings));
            return props;
        }
    }
}
