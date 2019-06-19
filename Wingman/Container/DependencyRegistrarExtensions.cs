namespace Wingman.Container
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    internal static class DependencyRegistrarExtensions
    {
        internal static void RegisterAllTypesOf<TService>(this IDependencyRegistrar dependencyRegistrar, Assembly assembly, Func<Type, bool> matchFilter = null, string key = null)
        {
            Type serviceType = typeof(TService);

            bool IsMatchNoFilter(Type type)
            {
                return serviceType.IsAssignableFrom(type) && !type.IsAbstract;
            }

            bool IsMatchWithFilter(Type type)
            {
                return IsMatchNoFilter(type) && matchFilter(type);
            }

            Type[] assemblyTypes = assembly.GetTypes();
            IEnumerable<Type> matchingAssemblyTypes = matchFilter == null ? assemblyTypes.Where(IsMatchNoFilter) : assemblyTypes.Where(IsMatchWithFilter);

            foreach (Type type in matchingAssemblyTypes)
            {
                dependencyRegistrar.RegisterSingleton(serviceType, type, key);
            }
        }
    }
}