using System.Collections.Generic;

namespace Pea.Core
{
	public interface IFitness
    {
        IEntity Entity { get; set; }
        bool IsValid { get; set; }
        int TournamentWinner { get; set; }
        int TournamentLoser { get; set; }
        bool IsEquivalent(IFitness other);
        bool IsLethal();
    }

    public interface IFitness<TF> : IFitness
    {
        IReadOnlyList<TF> Value { get; }
        double ConstraintViolation { get; }
    }
}
