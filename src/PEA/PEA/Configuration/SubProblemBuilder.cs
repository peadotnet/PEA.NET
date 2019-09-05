using Pea.Configuration.Implementation;

namespace Pea.Configuration
{
    public class SubProblemBuilder
    {
        protected SubProblem SubProblem;

        public SubProblemBuilder()
        {
            SubProblem = new SubProblem();
        }

        public SubProblemBuilder WithDecoder<DT>()
        {
            SubProblem.DecoderType = typeof(DT);
            return this;
        }

        public EncodingBuilder Encoding<CT>(string key) => new EncodingBuilder(SubProblem, key, typeof(CT));


        public SubProblem Build() => SubProblem;
    }
}
