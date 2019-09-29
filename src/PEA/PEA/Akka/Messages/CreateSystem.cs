using Pea.Configuration.Implementation;

namespace Pea.Akka.Messages
{
    public class CreateSystem
    {
        public PeaSettings Settings { get; }

        public CreateSystem(PeaSettings settings)
        {
            Settings = settings;
        }
    }
}
