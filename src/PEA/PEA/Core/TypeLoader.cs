using System;
using System.Reflection;

namespace Pea.Core
{
    public static class TypeLoader
    {
        public static object CreateInstance(Type classType)
		{
			var loadedClass = LoadClass(classType);
			var instance = Activator.CreateInstance(loadedClass);
			return instance;
		}

		public static object CreateInstance(Type classType, params object[] args)
		{
			var loadedClass = LoadClass(classType);
			var instance = Activator.CreateInstance(loadedClass, args);
			return instance;
		}

		public static Type LoadClass(Type classType)
		{
			if (classType == null) throw new ArgumentNullException(nameof(classType));

			var assembly = classType.Assembly;
			var loadedAssembly = Assembly.Load(assembly.FullName);
			var loadedClass = loadedAssembly.GetType(classType.FullName, true);
			return loadedClass;
		}
    }
}
