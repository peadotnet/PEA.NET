using Pea.Core;

namespace Pea.Chromosome.Implementation.StructVector.FieldLevelOperations
{
	public class IntegerFieldOperator<TS> : FieldOperatorBase<TS>, IFieldOperator<TS> where TS : struct
	{
		public IntegerFieldOperator(string fieldName, IRandom random, IParameterSet parameterSet) : base(fieldName, random, parameterSet)
		{
		}

		public object GetValue(TS instance)
		{
			return (int)Getter(instance);
		}

		public void Mutate(object instance)
		{
			throw new System.NotImplementedException();
		}

		public void SetValue(object instance, object newValue)
		{
			Setter(instance, newValue);
		}
	}
}
