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
        public List<IActorRef> Archipelagos { get; } = new List<IActorRef>();
        private int _receivedAcknowledgementsCount = 0;
        private IActorRef _starter;

        public PeaSystemActor()
        {
            Receive<CreateSystem>(m => CreateSystem(m.Settings));
            Receive<CreatedSuccessfully>(m=> CountCreatedArchipelagos());
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

            for (int a = 0; a < archipelagosCount; a++)
            {
                var archipelagoProps = ArchipelagoActor.CreateProps(settings);
                var actorRef = Context.ActorOf(archipelagoProps);
                Archipelagos.Add(actorRef);
                ArchipelagosCount++;
            }
        }

        public static Props CreateProps()
        {
            return Props.Create(() => new PeaSystemActor());
        }
    }
}
