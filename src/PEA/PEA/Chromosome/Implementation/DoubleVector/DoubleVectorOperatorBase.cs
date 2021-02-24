using Pea.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pea.Chromosome.Implementation.DoubleVector
{
    public class DoubleVectorOperatorBase
    {
        public IList<INeighborhoodConflictDetector> ConflictDetectors;

        protected readonly IRandom Random;
        protected readonly IParameterSet ParameterSet;

        public DoubleVectorOperatorBase(IRandom random, IParameterSet parameterSet, IList<INeighborhoodConflictDetector> conflictDetectors)
        {
            Random = random;
            ParameterSet = parameterSet;
            ConflictDetectors = conflictDetectors;
        }
    }
}
