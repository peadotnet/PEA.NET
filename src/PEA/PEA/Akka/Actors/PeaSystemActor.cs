using System.Collections.Generic;
using Akka.Actor;
using Pea.ActorModel.Messages;
using Pea.Akka.Messages;
using Pea.Core;
using Pea.Core.Island;

namespace Pea.Akka.Actors
{
    public class PeaSystemActor : ReceiveActor
    {
        public List<IActorRef> Archipelagos { get; } = new List<IActorRef>();
        private int ReceivedAcknowledgementsCount = 0;
        public int IslandsCount = 0;
        private IActorRef _starter;

        public PeaSystemActor()
        {
            Receive<CreateSystem>(m => CreateSystem(m.Settings));
            Receive<CreatedSuccessfully>(m=> CountCreatedIslands());
            Receive<InitEvaluator>(m => InitArchipelagos(m));
            Receive<PeaResult>(m => SendResultBack(m));
        }

        private void SendResultBack(PeaResult result)
        {
            _starter.Tell(result);
        }

        private void InitArchipelagos(InitEvaluator initMessage)
        {
            for (int i = 0; i < Archipelagos.Count; i++)
            {
                Archipelagos[i].Tell(initMessage, Sender);
            }
        }

        private void CountCreatedIslands()
        {
            ReceivedAcknowledgementsCount++;
            if (ReceivedAcknowledgementsCount == IslandsCount)
            {
                _starter.Tell(new CreatedSuccessfully());
            }
        }

        private void CreateSystem(Pea.Configuration.Implementation.PeaSettings settings)
        {
            _starter = Sender;
            //var parameterSet = new ParameterSet(settings.ParameterSet);
            var archipelagosCount = 1;// parameterSet.GetInt(ParameterNames.ArchipelagosCount);

            for (int a = 0; a < archipelagosCount; a++)
            {
                var islandProps = IslandActor.CreateProps(settings);
                var actorRef = Context.ActorOf(islandProps);
                Archipelagos.Add(actorRef);
                IslandsCount++;
            }
        }

        public static Props CreateProps()
        {
            return Props.Create(() => new PeaSystemActor());
        }
    }
}
