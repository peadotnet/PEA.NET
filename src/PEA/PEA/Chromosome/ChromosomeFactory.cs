using Pea.Engine;

namespace Pea.Chromosome
{
    public abstract class ChromosomeFactory : IEngineModifier
    {
        public abstract IEngine Apply(IEngine engine);
    }
}
