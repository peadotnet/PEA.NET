using System;
using Pea.Configuration.Implementation;
using Pea.Core;

namespace Pea.Configuration
{
    public class EncodingBuilder : SubProblemBuilder
    {
        public EncodingBuilder(SubProblem subProblem, string key, Type implementingType)
        {
            SubProblem = subProblem;
            SubProblem.Encoding = new Encoding
            {
                Key = key,
                ChromosomeType = implementingType
            };
        }

        public AlgorithmBuilder WithAlgorithm<T>() => new AlgorithmBuilder(SubProblem, SubProblem.Encoding.Key, typeof(T));

        public ActionListBuilder<IOperator, EncodingBuilder> Operators =>
            new ActionListBuilder<IOperator, EncodingBuilder>(SubProblem.Encoding.Operators, this);

        public ActionListBuilder<IChromosomeCreator, EncodingBuilder> Creators =>
            new ActionListBuilder<IChromosomeCreator, EncodingBuilder>(SubProblem.Encoding.Creators, this);
    }
}
