using System;
using Pea.Configuration.Implementation;

namespace Pea.Configuration
{
    public class PeaSettings
    {
        public Encoding Encoding { get; set; }

        public Type DecoderType { get; set; }

        public Type NichingType { get; set; }
    }
}
