using Akka.Actor;
using Pea.Core;

namespace Pea.ActorModel.Actors
{
    public class PhenotypeDecoderActor : ReceiveActor
    {
        private PeaSettings Settings { get; }

        public PhenotypeDecoderActor(PeaSettings settings)
        {
            Settings = settings;
        }

        public static Props CreateProps(PeaSettings settings)
        {
            var props = Props.Create(() => new PhenotypeDecoderActor(settings));
            return props;
        }
    }
}
