using System.Collections.Generic;
using Akka.Actor;
using Pea.ActorModel.Messages;
using Pea.Core;
using Pea.Island;

namespace Pea.ActorModel.Actors
{
    public class PeaSystemActor : ReceiveActor
    {
        public List<IActorRef> Archipelagos { get; } = new List<IActorRef>();

        public PeaSystemActor()
        {
            Receive<CreateSystem>(m => CreateSystem(m.Settings));

        }

        private void CreateSystem(PeaSettings settings)
        {
            var parameterSet = new ParameterSet(settings.ParameterSet);
            var archipelagosCount = parameterSet.GetInt(ParameterNames.ArchipelagosCount);

            for (int a = 0; a < archipelagosCount; a++)
            {
                var islandProps = IslandActor.CreateProps(settings);
                var actorRef = Context.ActorOf(islandProps);
                Archipelagos.Add(actorRef);
            }
        }

        public static Props CreateProps()
        {
            return Props.Create(() => new PeaSystemActor());
        }
    }
}
