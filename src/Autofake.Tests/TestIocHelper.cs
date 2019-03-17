using FluffySpoon.Testing.Autofake;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Autofake.Tests
{
    public static class TestIocHelper
    {
        public static IServiceProvider GetServiceProviderForUnit<TUnit>()
        {
            var services = IocHelper.CreateServiceCollection();

            var faker = new Autofaker();
            faker.UseMicrosoftDependencyInjection(services);
            faker.UseNSubstitute();

            faker.RegisterFakesForConstructorParameterTypesOf<TUnit>();

            return services.BuildServiceProvider();
        }
    }
}
