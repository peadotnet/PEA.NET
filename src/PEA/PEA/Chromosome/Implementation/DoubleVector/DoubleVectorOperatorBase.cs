using Pea.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pea.Chromosome.Implementation.DoubleVector
{
    public class DoubleVectorOperatorBase
    {
        public IList<IConflictDetector> ConflictDetectors;

        protected readonly IRandom Random;

        protected readonly IParameterSet ParameterSet;

        public DoubleVectorOperatorBase(IRandom random, IParameterSet parameterSet, IList<IConflictDetector> conflictDetectors)
        {
            Random = random;
            ParameterSet = parameterSet;
            ConflictDetectors = conflictDetectors;
        }

        /// <summary>
        /// Indicates whether the given value gets into conflict with the value
        /// on the left of the target position
        /// </summary>
        /// <param name="targetSection"></param>
        /// <param name="targetPositionIndex"></param>
        /// <param name="geneValue"></param>
        /// <returns></returns>
        public bool ConflictDetected(int position, double value)
        {
            for (int i=0; i< ConflictDetectors.Count; i++)
			{
                if (((IConflictDetector<int, double>)ConflictDetectors[i]).ConflictDetected(position, value)) return true;
			}
            return false;
        }
    }
}
