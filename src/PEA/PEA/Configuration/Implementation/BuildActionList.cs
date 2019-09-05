using System.Collections.Generic;

namespace Pea.Configuration.Implementation
{
    public class BuildActionList<T> : SubProblemBuilder
    {
        public List<BuildAction<T>> Actions => new List<BuildAction<T>>();
    }
}
