using System;
using System.Linq;
using System.Reflection;
using TinyIoC;

namespace IoC
{
    /// <summary>
    /// Mechanism to register concrete types with the TinyIoC container.
    /// </summary>
    /// <remarks>
    /// TinyIoC registers all interfaces as singletons and all concrete
    /// classes as multi-instance by default.
    /// https://github.com/grumpydev/TinyIoC/wiki/Setup---getting-started
    /// https://github.com/grumpydev/TinyIoC/wiki/Registration---lifetimes
    /// </remarks>
    public abstract class IoCConfig
    {
        private bool _autoRegister;

        private readonly Lazy<Assembly[]> _iocAssemblies = new Lazy<Assembly[]>(() => AppDomain.CurrentDomain.GetAssemblies());

        /// <summary>
        /// Create new instance of IoCConfig with auto-register true.
        /// </summary>
        protected IoCConfig() : this(true)
        { }

        /// <summary>
        /// Create new instance of IoCConfig and set the auto-register value.
        /// </summary>
        /// <param name="isAutoRegister">Auto-register value to set.</param>
        protected IoCConfig(bool isAutoRegister)
        {
            SetAutoRegister(isAutoRegister);
        }

        /// <summary>
        /// Get all assemblies to be scanned for IoC registrations.
        /// Default is the assemblies that have been loaded into the 
        /// execution context of this application domain.
        /// </summary>
        protected virtual Assembly[] IocAssemblies => _iocAssemblies.Value;

        /// <summary>
        /// Set the AutoRegister value to true to automatically register 
        /// all non-generic classes and interfaces in the current app domain;
        /// or to false to skip auto-registration.
        /// </summary>
        protected void SetAutoRegister(bool autoRegister)
        {
            _autoRegister = autoRegister;
        }

        /// <summary>
        /// Gets the current <see cref="TinyIoCContainer"/> container instance.
        /// Defaults to <see cref="TinyIoCContainer.Current"/>.
        /// </summary>
        public TinyIoCContainer Current { get; private set; } = TinyIoCContainer.Current;

        /// <summary>
        /// Perform all class registrations against the current <see cref="TinyIoCContainer"/> container instance.
        /// </summary>
        public virtual void Register()
        {
            // Automatically register all concrete types, abstract base classes, and interfaces
            if (_autoRegister)
            {
                Current.AutoRegister(IocAssemblies);
            }

            // Perform registrations designated in IIoCRegistrator instances
            AddIocRegistratorRegistrations(Current);

            // Register implementations
            AddRegistrations(Current);

            // Configure dependency resolvers
            AddDependencyResolvers(Current);
        }

        /// <summary>
        /// Sets the current <see cref="TinyIoCContainer"/> container to
        /// an alternate custom instance.
        /// </summary>
        /// <param name="container">Alternate <see cref="TinyIoCContainer"/> container.</param>
        public void SetCurrentContainer(TinyIoCContainer container)
        {
            Current = container;
        }

        /// <summary>
        /// Register concrete types or concrete implementations of interfaces/base classes.
        /// </summary>
        /// <param name="container">Current TinyIoCContainer instance</param>
        protected virtual void AddRegistrations(TinyIoCContainer container)
        {
        }

        /// <summary>
        /// Configure custom dependency resolvers.
        /// </summary>
        /// <param name="container">Current TinyIoCContainer instance</param>
        protected virtual void AddDependencyResolvers(TinyIoCContainer container)
        {
        }

        /// <summary>
        /// Perform class registrations against the current <see cref="TinyIoCContainer"/> container instance
        /// using a custom naming convention.
        /// </summary>
        /// <param name="contractNameFormat">Custom format to use for a specific interface naming convention.</param>
        /// <param name="implementationNameFormat">Custom format to use for a specific class naming convention.</param>
        protected virtual void RegisterByCustomConvention(string contractNameFormat, string implementationNameFormat)
        {
            Current.AutoRegister(IocAssemblies, contractNameFormat, implementationNameFormat);
        }

        private void AddIocRegistratorRegistrations(TinyIoCContainer container)
        {
            var iocRegistratorType = typeof(IIoCRegistrator);

            var implementationTypes = IocAssemblies
                .SelectMany(x => x.GetTypes())
                .Where(x => iocRegistratorType.IsAssignableFrom(x) && !x.IsInterface);

            foreach (var type in implementationTypes)
            {
                var instance = (IIoCRegistrator)Activator.CreateInstance(type);
                instance.AddRegistrations(container, IocAssemblies);
            }
        }
    }
}