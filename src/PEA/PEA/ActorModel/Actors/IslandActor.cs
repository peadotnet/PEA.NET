using Akka.Actor;
using Pea.ActorModel.Messages;
using Pea.Core;

namespace Pea.ActorModel.Actors
{
    public class IslandActor : ReceiveActor
    {
        private IEngine Engine { get; }

        public IslandActor(IEngine engine)
        {
            Engine = engine;
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
    }
}
