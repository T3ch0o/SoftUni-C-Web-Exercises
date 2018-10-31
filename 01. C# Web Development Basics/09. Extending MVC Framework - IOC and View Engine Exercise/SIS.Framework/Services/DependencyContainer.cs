namespace SIS.Framework.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using SIS.Framework.Services.Interfaces;

    public class DependencyContainer : IDependencyContainer
    {
        private readonly IDictionary<Type, Type> _dependencyMap;

        public DependencyContainer(IDictionary<Type, Type> dependencyMap)
        {
            _dependencyMap = dependencyMap;
        }

        private Type this[Type key] => _dependencyMap.ContainsKey(key) ? _dependencyMap[key] : null;

        public void RegisterDependency<TSource, TDestination>()
        {
            _dependencyMap[typeof(TSource)] = typeof(TDestination);
        }

        public T CreateInstance<T>() => (T)CreateInstance(typeof(T));

        public object CreateInstance(Type type)
        {
            Type typeInstance = this[type] ?? type;

            if (typeInstance.IsAbstract || typeInstance.IsInterface)
            {
                throw new InvalidOperationException($"Type {typeInstance.FullName} cannot be instantiated. Abstract type and interfaces cannot be instantiated.");
            }

            ConstructorInfo constructor = typeInstance
                .GetConstructors()
                .OrderByDescending(c => c.GetParameters().Length)
                .First();

            ParameterInfo[] constructorParameters = constructor.GetParameters();

            object[] constructorParametersObjects = new object[constructorParameters.Length];

            for (int index = 0; index < constructorParameters.Length; index++)
            {
                constructorParametersObjects[index] = CreateInstance(constructorParameters[index].ParameterType);
            }

            return constructor.Invoke(constructorParametersObjects);
        }
    }
}