using Pea.Configuration;
using Pea.Configuration.ProblemModels;
using Pea.Core;
using Xunit;

namespace Pea.Tests.Configuration
{
    public class ProblemModelTests
    {
        [Fact]
        public void TravelingSalesmanProblem_Apply_ShouldCreateSettings()
        {
            var settings = new PeaSettingsBuilder();
            settings.AddSubProblem("Berlin52", new TravelingSalesmanProblem(52))
                .AddConflictDetector<AllRightConflictDetector>();
            settings.StopWhen().TimeoutElapsed(10000);
        }

        [Fact]
        public void VehicleSchedulingProblem_Apply_ShouldCreateSettings()
        {
            var settings = new PeaSettingsBuilder();
            settings.AddSubProblem("Scheduling", new VehicleSchedulingProblem(2200))
                .AddConflictDetector<AllRightConflictDetector>();
        }


    }
}
