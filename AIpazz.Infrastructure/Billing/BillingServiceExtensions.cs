using Aipazz.Application.Billing.Interfaces;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System;
using System.Configuration;

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

                // Retrieve configuration values for Cosmos DB
                string? databaseName = configuration["CosmosDb:DatabaseName"];
                string? containerName = configuration["CosmosDb:ContainerName"];

                // Check if database name or container name is missing in the configuration
                if (string.IsNullOrEmpty(databaseName) || string.IsNullOrEmpty(containerName))
                {
                    throw new InvalidOperationException("Cosmos DB database or container name is not configured properly.");
                }

                // Return the repository instance with the required CosmosClient, databaseName, and containerName
                return new TimeEntryRepository(cosmosClient, databaseName, containerName);
            });

            // add additional repositories or services for Billing here if needed.

            return services;  
        }
    }
}
