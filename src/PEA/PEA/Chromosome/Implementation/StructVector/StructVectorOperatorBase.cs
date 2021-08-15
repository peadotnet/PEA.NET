using Pea.Core;
using System.Collections.Generic;

namespace Pea.Chromosome.Implementation.StructVector
{
	public class StructVectorOperatorBase<TS>
	{
        public IList<IConflictDetector> ConflictDetectors;

        protected readonly IRandom Random;

        protected readonly IParameterSet ParameterSet;

        public StructVectorOperatorBase(IRandom random, IParameterSet parameterSet, IList<IConflictDetector> conflictDetectors)
        {
            Random = random;
            ParameterSet = parameterSet;
            ConflictDetectors = conflictDetectors;
        }
    }
}
