using System.Collections.Generic;

namespace Pea.Core
{
	public interface IFitness
    {
        IEntity Entity { get; set; }
        bool IsValid { get; set; }
        bool IsEquivalent(IFitness other);
    }

    public interface IFitness<TF> : IFitness
    {
        IReadOnlyList<TF> Value { get; }
    }
}
