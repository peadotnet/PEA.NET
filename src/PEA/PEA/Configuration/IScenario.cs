namespace Pea.Configuration
{
    public interface IScenario
    {
        SubProblemBuilder Apply(string key, SubProblemBuilder subProblemBuilder);
    }
}
