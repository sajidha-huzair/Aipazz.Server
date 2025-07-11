using Aipazz.Application.Admin.Handler;
using Aipazz.Application.Admin.Interface;
using Aipazz.Application.client.Interfaces;
using Aipazz.Application.DocumentMGT.documentmgt.Commands;
using Aipazz.Application.DocumentMGT.Interfaces;
using Aipazz.Application.Matters.Interfaces;
using Aipazz.Domian;
using Aipazz.Infrastructure.client;
using Aipazz.Infrastructure.Matters.Tasks;
using AIpazz.Infrastructure.Documentmgt.Services;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;


namespace AIpazz.Infrastructure.DependencyInjection
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<IWordGenerator, WordGenerator>();
            services.AddScoped<IDocumentStorageService, DocumentStorageService>();
            services.AddScoped<IAdminService, AdminService>();
            services.AddSingleton<IClientRepository>(sp =>
                new ClientRepository(
                    sp.GetRequiredService<CosmosClient>(),
                    sp.GetRequiredService<IOptions<CosmosDbOptions>>()));

            services.AddSingleton<ITaskRepository>(sp =>
                new TaskRepository(
                    sp.GetRequiredService<CosmosClient>(),
                    sp.GetRequiredService<IOptions<CosmosDbOptions>>()));


            return services;
        }
    }
}
