﻿namespace Pea.Core
{
    public interface IChromosomeCreator
    {
        IChromosome Create();
    }

    public interface IChromosomeCreator<out TC> : IChromosomeCreator where TC : IChromosome
    {
        new IChromosome Create();
    }
}
