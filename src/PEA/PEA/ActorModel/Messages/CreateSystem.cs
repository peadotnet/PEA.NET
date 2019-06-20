using Pea.Core;

namespace Pea.ActorModel.Messages
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
