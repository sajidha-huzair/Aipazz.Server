using Aipazz.Application.Matters.Interfaces;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Aipazz.Domian;

namespace Aipazz.Infrastructure.Matters
{
    public static class MatterServiceExtensions
    {
        public static IServiceCollection AddMatterServices(this IServiceCollection services)
        {
            services.AddScoped<IMatterRepository, MatterRepository>();
            return services;
        }
    }
}

