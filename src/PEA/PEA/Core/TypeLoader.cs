using System;
using System.Reflection;

namespace Pea.Core
{
    public static class TypeLoader
    {
        public static object CreateInstance(Type classType)
        {
            if (classType == null) throw new ArgumentNullException(nameof(classType));

            var assembly = classType.Assembly;
            var loadedAssembly = Assembly.Load(assembly.FullName);
            var loadedClass = loadedAssembly.GetType(classType.FullName, true);
            var instance = Activator.CreateInstance(loadedClass);
            return instance;
        }
    }
}
