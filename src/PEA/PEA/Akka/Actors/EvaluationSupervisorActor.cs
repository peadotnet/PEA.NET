using System.Collections.Generic;
using System.Linq;
using Akka.Actor;
using Akka.Routing;
using Pea.ActorModel.Messages;
using Pea.Core;
using Pea.Core.Island;

namespace Pea.Akka.Actors
{
    public class EvaluationSupervisorActor : ReceiveActor, IWithUnboundedStash
    {
        public IStash Stash { get; set; }

        public IList<IEntity> EntityList { get; private set; }

        public MultiKey IslandKey { get; }

        public int EntityProcessingCount { get; private set; }
        public ParameterSet Parameters { get; }

        public IActorRef RequestSender { get; private set; }
        public IActorRef EvaluationRouter { get; private set; }
        public IActorRef[] EvaluationWorkers { get; private set; }

        public PeaSettings Settings { get; }

        public EvaluationSupervisorActor(MultiKey islandKey, PeaSettings settings)
        {
            IslandKey = islandKey;
            Settings = settings;
            Parameters = new ParameterSet(Settings.ParameterSet);
            Become(Idle);
        }

        void Idle()
        {
            Receive<InitEvaluator>(m => InitEvaluator(m));
            Receive<IList<IEntity>>(l => StartProcessing(l));
        }

        public void InitEvaluator(InitEvaluator m)
        {
            var props = EvaluationWorkerActor.CreateProps(IslandKey, Settings, m.InitData);
            var evaluatorsCount = Parameters.GetInt(ParameterNames.EvaluatorsCount);
            EvaluationWorkers = CreateWorkers(props, evaluatorsCount);

            var paths = EvaluationWorkers.Select(w => w.Path.ToString());
            var routerProps = Props.Empty.WithRouter(new RoundRobinGroup(paths));

            EvaluationRouter = Context.ActorOf(routerProps, "Evaluators");
        }

        public IActorRef[] CreateWorkers(Props props, int count)
        {
            var workers = new IActorRef[count];
            for (int i = 0; i < count; i++)
            {
                string name = $"Evaluator_{i}";
                var worker = Context.ActorOf(props, name);
                workers[i] = worker;
            }

            return workers;
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
                EvaluationRouter.Tell(entity);
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

        public static Props CreateProps(MultiKey islandKey, PeaSettings settings)
        {
            return Props.Create(() => new EvaluationSupervisorActor(islandKey, settings));
        }
    }
}
