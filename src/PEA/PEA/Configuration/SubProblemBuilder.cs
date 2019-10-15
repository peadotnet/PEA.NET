using Pea.Configuration.Implementation;
using Pea.Core;

namespace Pea.Configuration
{
    public class SubProblemBuilder
    {
        public SubProblem SubProblem;

        public SubProblemBuilder()
        {
            SubProblem = new SubProblem();
        }

        public EncodingBuilder WithEncoding<CT>(string key) => new EncodingBuilder(SubProblem, key, typeof(CT));

        public SubProblemBuilder WithDecoder<DT>() where DT: IEvaluation
        {
            SubProblem.Decoder = typeof(DT);
            return this;
        }

        public SubProblemBuilder WithNiching<NT>() where NT: INiching
        {
            SubProblem.Niching = typeof(NT);
            return this;
        }

        public SubProblemBuilder AddConflictDetector<CDT>() where CDT: IConflictDetector
        {
            SubProblem.ConflictDetectors.Add(typeof(CDT));
            return this;
        }

        public SubProblemBuilder SetParameter(string parameterKey, double parameterValue)
        {
            SubProblem.ParameterSet.Add(new PeaSettingsNamedValue(parameterKey, parameterValue));
            return this;
        }

        public SubProblem Build() => SubProblem;
    }
}
