using System;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using IoC;

namespace Scheduler.Runner
{
    public class ConsoleIoCConfig : IoCConfig
    {
        private readonly Lazy<Assembly[]> _iocAssemblies = new Lazy<Assembly[]>(() =>
        {
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            AssemblyLoadContext.Default.LoadFromAssemblyPath(Path.Combine(path, "Scheduler.Infrastructure.dll"));
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            return assemblies;
        });

        protected override Assembly[] IocAssemblies => _iocAssemblies.Value;

        public static ConsoleIoCConfig Instance { get; } = new ConsoleIoCConfig();
    }
}