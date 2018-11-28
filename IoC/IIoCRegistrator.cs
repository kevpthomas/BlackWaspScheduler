using System.Reflection;
using TinyIoC;

namespace IoC
{
    /// <summary>
    /// Provides mechanism to add custom registrations to a <see cref="TinyIoCContainer"/> container instance.
    /// </summary>
    public interface IIoCRegistrator
    {
        /// <summary>
        /// Add custom registrations to a provided container from a set of provided assemblies.
        /// </summary>
        /// <param name="container"><see cref="TinyIoCContainer"/> container instance.</param>
        /// <param name="allAssemblies">Assemblies containing the types to be registered.</param>
        void AddRegistrations(TinyIoCContainer container, Assembly[] allAssemblies);
    }
}