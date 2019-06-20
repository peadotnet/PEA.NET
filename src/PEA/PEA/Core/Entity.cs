namespace Pea.Core
{
    public class Entity : IEntity
    {
        public IGenotype Genotype { get; set; }
        public IPhenotype Phenotype { get; set; }
        public IFitness Fitness { get; set; }

        public object Clone()
        {
            return new Entity()
            {
                Genotype = this.Genotype,
                Phenotype = this.Phenotype,
                Fitness = this.Fitness
            };
        }
    }
}
