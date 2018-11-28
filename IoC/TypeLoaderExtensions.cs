using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TinyIoC;

namespace IoC
{
    /// <summary>
    /// Extension methods to assist with type registration to a TinyIoC container.
    /// </summary>
    public static class TypeLoaderExtensions
    {
        /// <summary>
        /// Attempt to automatically register all non-generic classes and interfaces by naming convention
        /// for the supplied assemblies.
        /// 
        /// If more than one class implements an interface then only one implementation will be registered
        /// although no error will be thrown.
        /// </summary>
        /// <param name="container">A <see cref="TinyIoCContainer"/> instance.</param>
        /// <param name="assemblies">Assemblies containing the interfaces to be implemented.</param>
        /// <param name="contractNameFormat">Custom format to use for a specific interface naming convention.</param>
        /// <param name="implementationNameFormat">Custom format to use for a specific class naming convention.</param>
        public static void AutoRegister(this TinyIoCContainer container,
            IEnumerable<Assembly> assemblies,
            string contractNameFormat,
            string implementationNameFormat)
        {
            if (assemblies == null) return;

            var contractAssemblyList = assemblies.ToList();
            if (!contractAssemblyList.Any()) return;

            var allPossibleImplementationTypes = new List<Type>();
            foreach (var assembly in contractAssemblyList)
            {
                allPossibleImplementationTypes.AddRange(assembly.GetTypes().Where(ti => ti.IsClass & !ti.IsAbstract && !ti.IsValueType && ti.IsVisible));
            }

            var allContractInterfaceTypes = contractAssemblyList.SelectMany(x => x.GetTypes()).Where(t => t.IsInterface).ToList();
            foreach (var contractInterfaceType in allContractInterfaceTypes)
            {
                var contractName = string.IsNullOrEmpty(contractNameFormat)
                    ? contractInterfaceType.Name
                    : string.Format(contractNameFormat, contractInterfaceType.Name);

                var implementationType = allPossibleImplementationTypes.FirstOrDefault(x => 
                    (string.IsNullOrEmpty(implementationNameFormat) ? $"I{x.Name}" : string.Format(implementationNameFormat, x.Name))
                    .Equals(contractName, StringComparison.OrdinalIgnoreCase));

                if (implementationType != null)
                {
                    container.Register(contractInterfaceType, implementationType);
                }
            }
        }

        /// <summary>
        /// Retrieves all loadable types from an assembly.
        /// </summary>
        /// <param name="assembly">Assembly containing defined types to be retrieved.</param>
        /// <param name="includeAbstractTypes">true to include abstract/interface types; otherwise false (default).</param>
        /// <returns>The types defined in the provided assembly.</returns>
        public static IEnumerable<Type> GetLoadableTypes(this Assembly assembly, 
            bool includeAbstractTypes = false)
        {
            try
            {
                return includeAbstractTypes
                    ? assembly.GetTypes()
                    : assembly.GetTypes().Where(x => !x.IsAbstract);
            }
            catch (ReflectionTypeLoadException e)
            {
                return includeAbstractTypes
                    ? e.Types.Where(t => t != null)
                    : e.Types.Where(t => t != null && !t.IsAbstract);
            }
        }

        /// <summary>
        /// Retrieves all loadable types from one or more assemblies.
        /// </summary>
        /// <param name="assemblies">Assembly collection containing defined types to be retrieved.</param>
        /// <param name="includeAbstractTypes">true to include abstract/interface types; otherwise false (default).</param>
        /// <returns>The types defined in the provided assemblies.</returns>
        public static IEnumerable<Type> GetLoadableTypes(this IEnumerable<Assembly> assemblies, 
            bool includeAbstractTypes = false)
        {
            return assemblies.SelectMany(s => s.GetLoadableTypes(includeAbstractTypes));
        }

        /// <summary>
        /// Retrieves all concrete types assignable from a specified type for an assembly.
        /// </summary>
        /// <typeparam name="TSource">Source type from which to return other derived types.</typeparam>
        /// <param name="assembly">Assembly containing defined types to be retrieved.</param>
        /// <returns>All assignable concrete types from the specified type.</returns>
        public static IEnumerable<Type> GetConcreteTypes<TSource>(this Assembly assembly)
            where TSource : class
        {
            return assembly
                .GetLoadableTypes()
                .Where(typeof(TSource).IsAssignableFrom)
                .ToList();
        }

        /// <summary>
        /// Retrieves all concrete types assignable from a specified type for an assembly collection.
        /// </summary>
        /// <typeparam name="TSource">Source type from which to return other derived types.</typeparam>
        /// <param name="assemblies">Assembly collection containing defined types to be retrieved.</param>
        /// <returns>All assignable concrete types from the specified type.</returns>
        public static IEnumerable<Type> GetConcreteTypes<TSource>(this IEnumerable<Assembly> assemblies)
            where TSource : class
        {
            return assemblies.SelectMany(x => x.GetConcreteTypes<TSource>());
        }

        /// <summary>
        /// Register multiple implementations of a type.
        /// </summary>
        /// <typeparam name="TRegisterType">Type that each implementation implements</typeparam>
        /// <param name="container">TinyIoC container to register with.</param>
        /// <param name="assemblies">
        /// Assembly collection containing defined types to be retrieved.
        /// Defaults to all current domain assemblies.
        /// </param>
        /// <returns>MultiRegisterOptions for the fluent API.</returns>
        public static TinyIoCContainer.MultiRegisterOptions RegisterMultiple<TRegisterType>(this TinyIoCContainer container, params Assembly[] assemblies)
            where TRegisterType : class
        {
            return container.RegisterMultiple<TRegisterType>((assemblies ?? AppDomain.CurrentDomain.GetAssemblies()).GetConcreteTypes<TRegisterType>());
        }
    }
}