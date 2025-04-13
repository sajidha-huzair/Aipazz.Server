using Aipazz.Application.Billing.Interfaces;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Configuration;
using Aipazz.Domian;

namespace AIpazz.Infrastructure.Billing
{
    public static class BillingServiceExtensions
    {
        public static IServiceCollection AddBillingServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Register TimeEntryRepository with Cosmos DB connection
            services.AddSingleton<ITimeEntryRepository, TimeEntryRepository>(serviceProvider =>
            {
                // Get the CosmosClient instance from the DI container
                var cosmosClient = serviceProvider.GetRequiredService<CosmosClient>();
                var options = serviceProvider.GetRequiredService<IOptions<CosmosDbOptions>>();

                return new TimeEntryRepository(cosmosClient, options);
            });

            // add additional repositories or services for Billing here if needed.

            return services;  
        }
    }
}
