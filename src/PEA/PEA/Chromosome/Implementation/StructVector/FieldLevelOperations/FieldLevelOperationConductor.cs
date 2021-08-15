using Pea.Core;

namespace Pea.Chromosome.Implementation.StructVector.FieldLevelOperations
{
	public class FieldLevelOperationConductor<TS> where TS: struct
	{
		IFieldOperator<TS>[] _fieldOperators;
		IRandom Random;
		IParameterSet ParameterSet;

		public FieldLevelOperationConductor(IRandom random, IParameterSet parameterSet)
		{
			Random = random;
			ParameterSet = parameterSet;

			var type = typeof(TS);
			var fields = type.GetFields();
			_fieldOperators = new IFieldOperator<TS>[fields.Length];
			for(int i=0; i < fields.Length; i++)
			{
				var name = fields[i].Name;
				var fieldType = fields[i].FieldType;
				if (fieldType == typeof(double))
				{
					_fieldOperators[i] = new DoubleFieldOperator<TS>(name, random, parameterSet);
				}
				else if (fieldType == typeof(bool))
				{
					_fieldOperators[i] = new BitFieldOperator<TS>(name, random, parameterSet);
				}
				else
				{
					_fieldOperators[i] = new IntegerFieldOperator<TS>(name, random, parameterSet);
				}
			}
		}
			
		public object GetValue(TS instance, int index)
		{
			return _fieldOperators[index].GetValue(instance);
		}

		public void SetValue(TS instance, int index, object newValue)
		{
			_fieldOperators[index].SetValue(instance, newValue);
		}

		public void Mutate(TS instance, int index)
		{
			_fieldOperators[index].Mutate(instance);
		}
	}
}
