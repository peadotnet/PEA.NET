using System;
using System.Collections.Generic;
using System.Text;

namespace Pea.Core
{
    public interface IChromosomeCreator
    {
        IChromosome Create();
    }

    public interface IChromosomeCreator<TC> : IChromosomeCreator where TC : IChromosome
    {
        new IChromosome Create();
    }
}
