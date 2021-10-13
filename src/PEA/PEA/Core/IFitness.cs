using System.Collections.Generic;

namespace Pea.Core
{
	public interface IFitness
    {
        IEntity Entity { get; set; }
        int TournamentWinner { get; set; }
        int TournamentLoser { get; set; }
        bool IsEquivalent(IFitness other);
        bool IsLethal();
        IReadOnlyList<double> Value { get; }
        double ConstraintViolation { get; }
    }
}
