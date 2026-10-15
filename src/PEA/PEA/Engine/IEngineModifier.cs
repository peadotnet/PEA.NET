namespace Pea.Engine
{
    public interface IEngineModifier
    {
        IEngine Apply(IEngine engine);
    }
}
