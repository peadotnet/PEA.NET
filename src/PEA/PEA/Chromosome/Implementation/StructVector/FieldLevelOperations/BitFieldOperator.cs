using Pea.Core;

namespace Pea.Chromosome.Implementation.StructVector.FieldLevelOperations
{
	public class BitFieldOperator<TS> : FieldOperatorBase<TS>, IFieldOperator<TS> where TS : struct
	{
		public BitFieldOperator(string fieldName, IRandom random, IParameterSet parameterSet) : base(fieldName, random, parameterSet)
		{
		}

		public object GetValue(TS instance)
		{
			return Getter(instance);
		}

		public void Mutate(object instance)
		{
			var value = (bool)Getter((TS)instance);
			Setter(instance, !value);
		}

		public void SetValue(object instance, object newValue)
		{
			Setter(instance, newValue);
		}
	}
}
