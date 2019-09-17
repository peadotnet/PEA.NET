namespace Pea.Configuration
{
    public interface IProblemModel
    {
        SubProblemBuilder Apply(string key, SubProblemBuilder subProblemBuilder);
    }
}
