using System;
using System.Data.Common;
using Pea.Configuration.Implementation;

namespace Pea.Configuration
{
    public class EncodingBuilder : SubProblemBuilder
    {
        public EncodingBuilder(SubProblem subProblem, string key, Type chromosomeType)
        {
            SubProblem = subProblem;
            SubProblem.Encoding = new Encoding
            {
                Key = key,
                ChromosomeType = chromosomeType
            };
        }

        public ActionListBuilder<Type, EncodingBuilder> Operators => new ActionListBuilder<Type, EncodingBuilder>(SubProblem.Encoding.Operators, this);

        //public EncodingBuilder AddOperator<T>()
        //{
        //    var op = new BuildAction<Type>(ActionTypes.Add, typeof(T));
        //    SubProblem.Encoding.Operators.Add(op);
        //    return this;
        //}

        //public EncodingBuilder ClearOperator()
        //{
        //    var op = new BuildAction<Type>(ActionTypes.Clear, null);
        //    SubProblem.Encoding.Operators.Add(op);
        //    return this;
        //}

        //public EncodingBuilder RemoveOperator<T>()
        //{
        //    var op = new BuildAction<Type>(ActionTypes.Remove, typeof(T));
        //    SubProblem.Encoding.Operators.Add(op);
        //    return this;
        //}
    }
}
