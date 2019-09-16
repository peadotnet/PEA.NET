using System;
using Pea.Configuration.Implementation;
using Pea.Core;

namespace Pea.Configuration
{
    public class AlgorithmBuilder: EncodingBuilder
    {
        public AlgorithmBuilder(SubProblem subProblem, string key, Type implementingType) : base(subProblem, key, implementingType)
        {
            SubProblem = subProblem;
            SubProblem.Encoding.Algorithm = new Implementation.Algorithm(implementingType);
        }

        public ActionListBuilder<ISelection, AlgorithmBuilder> Selections =>
            new ActionListBuilder<ISelection, AlgorithmBuilder>(SubProblem.Encoding.Algorithm.Selections, this);

        public ActionListBuilder<IReinsertion, AlgorithmBuilder> ReInsertions =>
            new ActionListBuilder<IReinsertion, AlgorithmBuilder>(SubProblem.Encoding.Algorithm.Reinsertions, this);
    }
}
