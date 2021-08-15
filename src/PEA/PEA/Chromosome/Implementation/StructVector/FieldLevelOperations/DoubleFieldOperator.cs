using Pea.Core;

namespace Pea.Chromosome.Implementation.StructVector.FieldLevelOperations
{
	public class DoubleFieldOperator<TS> : FieldOperatorBase<TS>, IFieldOperator<TS> where TS : struct
	{
		double _mutationIntensity;

		public DoubleFieldOperator(string fieldName, IRandom random, IParameterSet parameterSet) : base(fieldName, random, parameterSet)
		{
			_mutationIntensity = parameterSet.GetValue(ParameterNames.MutationIntensity);
		}

		public object GetValue(TS instance)
		{
			return (double)Getter(instance);
		}

		public void Mutate(object instance)
		{
			var value = (double)Getter((TS)instance);
			var newValue = Random.GetGaussian(value, _mutationIntensity);
			Setter(instance, newValue);
		}

		public void SetValue(object instance, object newValue)
		{
			Setter(instance, newValue);
		}
	}
}
