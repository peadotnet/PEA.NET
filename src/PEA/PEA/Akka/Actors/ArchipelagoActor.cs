using System.Collections.Generic;
using Akka.Actor;
using Pea.Configuration.Implementation;
using Pea.Core;
using Pea.Core.Island;

namespace Pea.Akka.Actors
{
    public class ArchipelagoActor : ReceiveActor
    {
        private PeaSettings Settings;
        List<IActorRef> Islands = new List<IActorRef>();

        public ArchipelagoActor(PeaSettings settings)
        {
            Settings = settings;
            
        }

        protected override void PreStart()
        {
            CreateIslands(Settings);
            base.PreStart();
        }

        private void CreateIslands(PeaSettings settings)
        {
            var parameterSet = new ParameterSet(settings.ParameterSet);
            var islandsCount = parameterSet.GetInt(ParameterNames.IslandsCount);

            for (int i = 0; i < islandsCount; i++)
            {
                var islandProps = IslandActor.CreateProps(settings);
                var actorRef = Context.ActorOf(islandProps);
                Islands.Add(actorRef);
            }
        }

        public static Props CreateProps(PeaSettings settings)
        {
            var props = Props.Create(() => new ArchipelagoActor(settings));
            return props;
        }
    }
}
