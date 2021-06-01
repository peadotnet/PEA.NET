using System;
using System.Collections.Generic;
using Akka.Actor;
using Pea.ActorModel.Messages;
using Pea.Akka.Messages;
using Pea.Configuration.Implementation;
using Pea.Core;
using Pea.Core.Island;

namespace Pea.Akka.Actors
{
    public class PeaSystemActor : ReceiveActor
    {
        public int ArchipelagosCount = 0;
        public List<IActorRef> Archipelagos;
        private int _receivedAcknowledgementsCount = 0;
        private IActorRef _starter;

        List<IEntity> Bests = new List<IEntity>();
        IFitnessComparer FitnessComparer;
        event NewEntitiesMergedToBestDelegate NewEntitiesMergedToBest;

        List<IEntity> Result = new List<IEntity>();
        int ResultsWaitForCount;

        public PeaSystemActor()
        {
            Receive<CreateSystem>(m => CreateSystem(m.Settings));
            Receive<CreatedSuccessfully>(m=> CountCreatedArchipelagos());
            Receive<InitEvaluator>(m => InitArchipelagos(m));
            Receive<Travel>(m => Transit(m));
            Receive<PeaResult>(m => CollectResults(m));
        }

		private void Transit(Travel travel)
		{
			if (travel.TravelerType == Migration.TravelerTypes.Best)
			{
                MergeToBests(travel.Members);
			}
		}

		private void AddCallbackEvents(List<NewEntitiesMergedToBestDelegate> delegates)
        {
            if (delegates.Count > 0)
            {
                for (int d = 0; d < delegates.Count; d++)
                {
                    NewEntitiesMergedToBest += delegates[d];
                }
            }
        }

        public void MergeToBests(IList<IEntity> entities)
        {
            if (NewEntitiesMergedToBest == null) return;

            bool anyMerged = false;
            for (int e = 0; e < entities.Count; e++)
            {
                anyMerged |= FitnessComparer.MergeToBests(Bests, entities[e]);
            }
            if (anyMerged) NewEntitiesMergedToBest(Bests);
        }

        public void MergeToResults(IList<IEntity> entities)
		{
            for (int e = 0; e < entities.Count; e++)
            {
                FitnessComparer.MergeToBests(Result, entities[e]);
            }
        }

        private void NewEntitiesMergetToBestCallback(IList<IEntity> bests)
        {
            if (NewEntitiesMergedToBest != null) NewEntitiesMergedToBest(bests);
        }

        private void CollectResults(PeaResult result)
        {
            MergeToResults(result.BestSolutions);
            ResultsWaitForCount--;
            if (ResultsWaitForCount == 0)
            {
                var finalResult = new PeaResult(result.StopReasons, Result);
                _starter.Tell(finalResult);
            }
        }

        private void InitArchipelagos(InitEvaluator initMessage)
        {
            for (int i = 0; i < Archipelagos.Count; i++)
            {
                Archipelagos[i].Tell(initMessage, Sender);
            }
        }

        private void CountCreatedArchipelagos()
        {
            _receivedAcknowledgementsCount++;
            if (_receivedAcknowledgementsCount == ArchipelagosCount)
            {
                _starter.Tell(new CreatedSuccessfully());
            }
        }

        private void CreateSystem(PeaSettings settings)
        {
            _starter = Sender;

            var parameterSet = new ParameterSet(settings.ParameterSet);
            int archipelagosCount = parameterSet.GetInt(ParameterNames.ArchipelagosCount);
            int islandsCount = parameterSet.GetInt(ParameterNames.IslandsCount);

            ResultsWaitForCount = archipelagosCount * islandsCount;

            Archipelagos = new List<IActorRef>(archipelagosCount);

            for (int a = 0; a < archipelagosCount; a++)
            {
                var archipelagoProps = ArchipelagoActor.CreateProps(settings);
                var actorRef = Context.ActorOf(archipelagoProps);
                Archipelagos.Add(actorRef);
                ArchipelagosCount++;
            }

            var fitness = (IFitnessFactory)Activator.CreateInstance(settings.Fitness);
            FitnessComparer = fitness.GetFitnessComparer();

            AddCallbackEvents(settings.NewEntityMergedToBest);
        }

        public static Props CreateProps()
        {
            return Props.Create(() => new PeaSystemActor());
        }
    }
}
