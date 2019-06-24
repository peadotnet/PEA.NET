using System.Collections.Generic;
using Akka.Actor;
using Akka.Routing;
using Pea.Core;

namespace Pea.ActorModel.Actors
{
    public class FitnessSupervisorActor : ReceiveActor, IWithUnboundedStash
    {
        public IStash Stash { get; set; }

        public IList<IEntity> EntityList { get; private set; }

        public int EntityProcessingCount { get; private set; }

        public IActorRef RequestSender { get; private set; }
        public IActorRef FitnessEvaluators { get; private set; }

        public PeaSettings Settings { get; }

        public FitnessSupervisorActor(PeaSettings settings)
        {
            Settings = settings;
        }

        protected override void PreStart()
        {
            ParameterSet parameterSet = new ParameterSet(Settings.ParameterSet);
            var evaluatorsCount = parameterSet.GetInt(Island.ParameterNames.FitnessEvaluatorsCount);
            var props = FitnessEvaluatorActor.CreateProps(Settings)
                .WithRouter(new RoundRobinPool(evaluatorsCount));

            FitnessEvaluators = Context.ActorOf(props, "FitnessEvaluators");

            base.PreStart();
        }

        void Idle()
        {
            Receive<IList<IEntity>>(l => StartProcessing(l));
        }

        void Processing()
        {
            Receive<IList<IEntity>>(l => Stash.Stash());
            Receive<IEntity>(e => Collect(e));
        }

        private void StartProcessing(IList<IEntity> entityList)
        {
            RequestSender = Sender;
            EntityList = entityList;
            EntityProcessingCount = 0;
            for (int i=0; i < entityList.Count; i++)
            {
                var entity = entityList[i];
                entity.IndexOfList = i;
                FitnessEvaluators.Tell(entity);
                EntityProcessingCount++;
            }

            Become(Processing);
        }

        private void Collect(IEntity entity)
        {
            var index = entity.IndexOfList;
            EntityList[index] = entity;
            EntityProcessingCount--;
            if (EntityProcessingCount == 0)
            {
                RequestSender.Tell(EntityList);
                Stash.UnstashAll();
                Become(Idle);
            }
        }
    }
}
