namespace Pea.Core
{
    public interface IEngineModifier
    {
        IEngine Apply(IEngine engine);
    }
}
