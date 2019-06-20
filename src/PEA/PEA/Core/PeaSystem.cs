using System;
using System.Collections.Generic;

namespace Pea.Core
{
    public class PeaSystem
    {
        public PeaSettings Settings { get; set; } = new PeaSettings();

        private PeaSystem()
        {
            
        }

        public static PeaSystem Create()
        {
            return new PeaSystem();
        }

        public void AddChromosome(string name, IChromosomeFactory chromosome)
        {
            Settings.Chromosomes.Add(new KeyValuePair<string, IChromosomeFactory>(name, chromosome));
        }

        public void WithCreator(IEntityCreator creator, double probability = 1.0)
        {
            Settings.EntityCreators.Add(new KeyValuePair<Type, double>(creator.GetType(), probability));
        }

        public void AddSelection(ISelection selection, double probability = 1.0)
        {
            Settings.Selectors.Add(new KeyValuePair<Type, double>(selection.GetType(), probability));
        }

        public void AddReinsertion(IReinsertion reinsertion, double probability = 1.0)
        {
            Settings.Reinsertions.Add(new KeyValuePair<Type, double>(reinsertion.GetType(), probability));
        }

        public void WithPhenotypeDecoder(IPhenotypeDecoder phenotypeDecoder)
        {
            Settings.PhenotypeDecoder = phenotypeDecoder.GetType();
        }

        public void WithFitness(IFitnessFactory fitness)
        {
            Settings.Fitness = fitness.GetType();
        }

        public void SetParameter(string name, double value)
        {
            Settings.ParameterSet.Add(new KeyValuePair<string, double>(name, value));
        }



        public void Start()
        {

        }
    }
}
