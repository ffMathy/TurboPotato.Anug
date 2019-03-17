using Microsoft.Extensions.DependencyInjection;
using FluffySpoon.Extensions.MicrosoftDependencyInjection;

namespace Autofake
{
    public static class IocHelper
    {
        public static IServiceCollection CreateServiceCollection()
        {
            var services = new ServiceCollection();
            services.AddAssemblyTypesAsImplementedInterfaces(
                typeof(IocHelper).Assembly);

            return services;
        }
    }
}
