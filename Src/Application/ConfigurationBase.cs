using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public abstract class ConfigurationBase 
{
    public abstract void ConfigureServices(IServiceCollection services);

    public static void ConfigureServicesFromAssemblies(IServiceCollection services, IEnumerable<string> assemblies)
    {
        ConfigureServicesFromAssemblies(services, assemblies.Select(Assembly.Load));
    }


    public static void ConfigureServicesFromAssemblies(IServiceCollection services, IEnumerable<Assembly> assemblies)
    {
        assemblies
            .SelectMany(ass=> ass.GetTypes())
            .Where(type=>typeof(ConfigurationBase).IsAssignableFrom(type))
            .Where(type=> type is { IsInterface:false, IsAbstract: false })
            .Select(type=>(ConfigurationBase)Activator.CreateInstance(type)!)
            .ToList()
            .ForEach(hostingStartup =>
            {
                var name = hostingStartup.GetType().Name.Replace("Configure", "");
                Console.WriteLine($"[{DateTime.Now:hh:mm:ss} INF] ? Configuring {name}");
                hostingStartup.ConfigureServices(services);
            });
    }
}