namespace Pea.Chromosome.Implementation.StructVector.FieldLevelOperations
{
	public interface IFieldOperator<TS> where TS: struct
	{
		object GetValue(TS instance);
		void SetValue(object instance, object newValue);
		void Mutate(object instance);
	}
}
