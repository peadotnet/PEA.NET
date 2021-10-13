using System.Collections.Generic;

namespace Pea.Core
{
    public interface IEntity
    {
        IDictionary<string, IChromosome> Chromosomes { get; set; }
        IFitness Fitness { get; }
        MultiKey OriginIslandKey { get; }
        int IndexInList { get; set; }
        Dictionary<string, string> LastCrossOvers { get; set; }
        Dictionary<string, string> LastMutations { get; set; }
        void SetFitness(IFitness fitness);
        IEntity Clone(bool cloneChromosomes);
    }
}

