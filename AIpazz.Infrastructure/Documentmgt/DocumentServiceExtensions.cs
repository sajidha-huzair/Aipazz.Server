using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Aipazz.Application.DocumentMGT.Interfaces;
using Aipazz.Domian;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace AIpazz.Infrastructure.Documentmgt
{
    public static class DocumentServiceExtensions
    {
        public static IServiceCollection Addtemplateservivces(this IServiceCollection services, IConfiguration configuration) {
            services.AddSingleton<ITemplateRepository, TemplateRepository>(serviceprovider =>
            {
                var cosmosClient = serviceprovider.GetRequiredService<CosmosClient>();
                var options = serviceprovider.GetRequiredService<IOptions<CosmosDbOptions>>();
                return new TemplateRepository(cosmosClient, options);

            });
            return services;       
        }
    }
}
