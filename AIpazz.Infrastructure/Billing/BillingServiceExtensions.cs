using Aipazz.Application.Billing.Interfaces;
using Aipazz.Domian;
using AIpazz.Infrastructure.Billing;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Aipazz.Infrastructure.Billing
{
    public static class BillingServiceExtensions
    {
        /// <summary>
        /// Registers all billing-related services and repositories.
        /// </summary>
        public static IServiceCollection AddBillingServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Shared factory to register Cosmos-based repositories
            services.AddSingleton<ITimeEntryRepository>(sp =>
                new TimeEntryRepository(
                    sp.GetRequiredService<CosmosClient>(),
                    sp.GetRequiredService<IOptions<CosmosDbOptions>>()));

            services.AddSingleton<IExpenseEntryRepository>(sp =>
                new ExpenseEntryRepository(
                    sp.GetRequiredService<CosmosClient>(),
                    sp.GetRequiredService<IOptions<CosmosDbOptions>>()));

            services.AddSingleton<IInvoiceRepository>(sp =>
                new InvoiceRepository(
                    sp.GetRequiredService<CosmosClient>(),
                    sp.GetRequiredService<IOptions<CosmosDbOptions>>()));

            return services;
        }
    }
}
