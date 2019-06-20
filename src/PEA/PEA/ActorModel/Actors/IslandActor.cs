using Akka.Actor;
using Pea.ActorModel.Messages;
using Pea.Core;
using Pea.Island;

namespace Pea.ActorModel.Actors
{
    public class IslandActor : ReceiveActor
    {
        private IEngine Engine { get; }

        public IslandActor(PeaSettings settings)
        {
            Engine = IslandEngineFactory.Create(settings);
            Receive<Continue>(m => RunOneStep());
        }

        private void RunOneStep()
        {
            bool stop = Engine.RunOnce();
            if (!stop)
            {
                Self.Tell(Continue.Instance);
            }
        }

        public static Props CreateProps(PeaSettings settings)
        {
            var props = Props.Create(() => new IslandActor(settings));
            return props;
        }
    }
}
