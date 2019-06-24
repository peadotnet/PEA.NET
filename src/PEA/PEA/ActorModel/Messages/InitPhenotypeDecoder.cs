using Pea.Core;

namespace Pea.ActorModel.Messages
{
    public class InitPhenotypeDecoder
    {
        public IPhenotypeDecoderInitData InitData { get; }

        public InitPhenotypeDecoder(IPhenotypeDecoderInitData initData)
        {
            InitData = initData;
        }
    }
}
