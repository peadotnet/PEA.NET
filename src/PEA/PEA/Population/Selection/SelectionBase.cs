using System.Collections.Generic;
using Pea.Core;

namespace Pea.Selection
{
    public abstract class SelectionBase : ISelection
    {
        protected IRandom Random { get; }

        protected IFitnessComparer FitnessComparer { get; }

        protected IParameterSet ParameterSet { get; }

        protected SelectionBase(IRandom random, IFitnessComparer fitnessComparer, IParameterSet parameterSet)
        {
            Random = random;
            FitnessComparer = fitnessComparer;
            ParameterSet = parameterSet;
        }

        public abstract IEntityList Select(IEntityList entities, int count);
    }
}
