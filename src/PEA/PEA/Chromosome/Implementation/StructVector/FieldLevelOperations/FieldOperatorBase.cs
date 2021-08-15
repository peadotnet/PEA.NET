using Pea.Core;
using System;
using System.Linq.Expressions;

namespace Pea.Chromosome.Implementation.StructVector.FieldLevelOperations
{
	public class FieldOperatorBase<TS> where TS: struct
	{
		protected delegate void StructSetterAction(ref object instance, object value);
		protected Func<TS, object> Getter;
		protected Action<object, object> Setter;

		protected readonly IRandom Random;

		public FieldOperatorBase(string fieldName, IRandom random, IParameterSet parameterSet)
		{
			Random = random;
			Getter = BuildGetter(fieldName);
			Setter = BuildSetter(fieldName);
		}

		private Func<TS, object> BuildGetter(string fieldName)
		{
			var arg = Expression.Parameter(typeof(TS), "x");
			var field = Expression.Property(arg, fieldName);
			var converted = Expression.Convert(field, typeof(object));
			var getter = Expression.Lambda<Func<TS, object>>(converted, arg).Compile();
			return getter;
		}

		public static Action<object, object> BuildSetter(string propertyName)
		{
			var propertyInfo = typeof(TS).GetProperty(propertyName);
			// Note that we are testing whether this is a value type
			bool isValueType = propertyInfo.DeclaringType.IsValueType;
			var method = propertyInfo.GetSetMethod(true);
			var obj = Expression.Parameter(typeof(object), "instance");
			var value = Expression.Parameter(typeof(object));

			// Note that we are using Expression.Unbox for value types
			// and Expression.Convert for reference types
			Expression<Action<object, object>> expr =
				Expression.Lambda<Action<object, object>>(
					Expression.Call(
						isValueType ?
							Expression.Unbox(obj, method.DeclaringType) :
							Expression.Convert(obj, method.DeclaringType),
						method,
						Expression.Convert(value, method.GetParameters()[0].ParameterType)),
						obj, value);
			Action<object, object> action = expr.Compile();
			return action;
		}

	}
}
