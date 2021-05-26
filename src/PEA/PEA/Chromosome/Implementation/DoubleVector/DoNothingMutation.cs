using Pea.Core;
using System.Collections.Generic;

namespace Pea.Chromosome.Implementation.DoubleVector
{
    public class DoNothingMutation : DoubleVectorOperatorBase, IMutation<DoubleVectorChromosome>
    {
        public DoNothingMutation(IRandom random, IParameterSet parameterSet, IList<IConflictDetector> conflictDetectors)
            : base(random, parameterSet, conflictDetectors)
        {
        }

        public IChromosome Mutate(IChromosome chromosome)
        {
            return chromosome;
        }
    }
}
