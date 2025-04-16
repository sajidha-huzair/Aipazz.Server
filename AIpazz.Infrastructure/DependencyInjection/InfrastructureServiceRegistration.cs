using Aipazz.Application.DocumentMGT.Interfaces;
using AIpazz.Infrastructure.Documentmgt.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AIpazz.Infrastructure.DependencyInjection
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<IWordGenerator, WordGenerator>();
            return services;
        }
    }
}
