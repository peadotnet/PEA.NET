using Pea.Configuration.Implementation;

namespace Pea.Configuration
{
    public class PeaSettingsBuilder
    {
        public PeaSettings PeaSettings = new PeaSettings();

        public SubProblemBuilder SubProblems => new SubProblemBuilder();


    }
}
