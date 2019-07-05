using System.Collections.Generic;
using Akka.Actor;
using Akka.Routing;
using Pea.ActorModel.Messages;
using Pea.Core;

namespace Pea.Akka.Actors
{
    public class EvaluationSupervisorActor : ReceiveActor, IWithUnboundedStash
    {
        public IStash Stash { get; set; }

        public IList<IEntity> EntityList { get; private set; }

        public int EntityProcessingCount { get; private set; }
        public ParameterSet Parameters { get; }

        public IActorRef RequestSender { get; private set; }
        public IActorRef FitnessEvaluators { get; private set; }

        public PeaSettings Settings { get; }

        public EvaluationSupervisorActor(PeaSettings settings)
        {
            Settings = settings;
            Parameters = new ParameterSet(Settings.ParameterSet);
            Become(Idle);
        }

        void Idle()
        {
            Receive<InitEvaluator>(m => InitEvaluator(m));
            Receive<IList<IEntity>>(l => StartProcessing(l));
        }

        private void InitEvaluator(InitEvaluator m)
        {
            var evaluatorsCount = Parameters.GetInt(Island.ParameterNames.FitnessEvaluatorsCount);
            var props = EvaluationWorkerActor.CreateProps(Settings, m.InitData)
                .WithRouter(new RoundRobinPool(evaluatorsCount));

            FitnessEvaluators = Context.ActorOf(props, "FitnessEvaluators");
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

        public static Props CreateProps(PeaSettings settings)
        {
            return Props.Create(() => new EvaluationSupervisorActor(settings));
        }
    }
}
