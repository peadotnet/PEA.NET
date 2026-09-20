using Pea.Core.Entity;
using System.Collections.Generic;

namespace Pea.Core
{
	public interface IFitness
    {
        EntityBase Entity { get; set; }
        int TournamentWinner { get; set; }
        int TournamentLoser { get; set; }
        bool IsEquivalent(IFitness other);
        bool IsLethal();
        IReadOnlyList<double> Value { get; }
        double ConstraintViolation { get; }
    }
}
