using System;
using System.Collections.Generic;

namespace Pea.Core
{
    public class PeaSettings
    {
        public Type Algorithm { get; set; }

        public IList<KeyValuePair<string, IChromosomeFactory>> Chromosomes { get; set; } =
            new List<KeyValuePair<string, IChromosomeFactory>>();

        public IList<KeyValuePair<string, double>> ParameterSet { get; set; } =
            new List<KeyValuePair<string, double>>();

        public IList<KeyValuePair<Type, double>> EntityCreators { get; set; } =
            new List<KeyValuePair<Type, double>>();

        public IList<KeyValuePair<Type, double>> Selectors { get; set; } =
            new List<KeyValuePair<Type, double>>();

        public IList<KeyValuePair<Type, double>> Reinsertions { get; set; } =
            new List<KeyValuePair<Type, double>>();

        public Type Fitness { get; set; }

        public Type PhenotypeDecoder { get; set; }

        public IStopCriteria StopCriteria { get; set; }

        public Type Random { get; set; }
    }
}
