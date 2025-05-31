using Aipazz.Application.Matters.Interfaces;
using Aipazz.Domian;
using Aipazz.Infrastructure.Matters; // <-- Import for StatusRepository
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Aipazz.Infrastructure.Matters
{
    public static class MatterServiceExtensions
    {
        public static IServiceCollection AddMatterServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Register MatterRepository using scoped lifetime
            services.AddScoped<IMatterRepository, MatterRepository>();

            // Register StatusRepository with Cosmos DB connection
            services.AddSingleton<IStatusRepository, StatusRepository>(serviceProvider =>
            {
                var cosmosClient = serviceProvider.GetRequiredService<CosmosClient>();
                var options = serviceProvider.GetRequiredService<IOptions<CosmosDbOptions>>();
                return new StatusRepository(cosmosClient, options);
            });

            services.AddSingleton<StatusSeeder>();


            return services;
        }
    }
}
