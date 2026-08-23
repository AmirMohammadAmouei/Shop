using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Transportation.Buisness._0.Common.FileManager;

namespace Transportation.Buisness
{
    public static class DIBuisnessBootstrapper
    {
        public static IServiceCollection AddBuiseness(this IServiceCollection services)
        {
            services.AddBusinessServices();
            services.AddBusinessMappers();


            return services;
        }

        private static IServiceCollection AddBusinessServices(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();

            var serviceTypes = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.IsPublic && t.Name.EndsWith("Service"));

            foreach (var type in serviceTypes)
            {
                services.AddScoped(type);
            }

            return services;
        }

        private static IServiceCollection AddBusinessMappers(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();

            var mapperTypes = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.IsPublic && t.Name.EndsWith("Mapper"));

            foreach (var type in mapperTypes)
            {
                var matchingInterface = type.GetInterfaces()
                    .FirstOrDefault(i => i.Name == "I" + type.Name);

                if (matchingInterface != null)
                {
                    services.AddScoped(matchingInterface, type);
                }
                else
                {
                    services.AddScoped(type);
                }
            }

            return services;
        }
    }
}

