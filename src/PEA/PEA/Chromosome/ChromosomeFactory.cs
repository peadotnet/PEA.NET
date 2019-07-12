using Pea.Core;

namespace Pea.Chromosome
{
    public abstract class ChromosomeFactory : IEngineModifier
    {
        public abstract IEngine Apply(IEngine engine);
    }
}
