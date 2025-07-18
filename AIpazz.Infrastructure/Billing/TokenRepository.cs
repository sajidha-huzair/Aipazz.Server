using Aipazz.Application.Billing.Interfaces;
using Aipazz.Domian;
using Aipazz.Domian.Billing;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Threading.Tasks;

namespace AIpazz.Infrastructure.Billing
{
    public class TokenRepository : ITokenRepository
    {
        private readonly Container _container;

        public TokenRepository(CosmosClient client, IOptions<CosmosDbOptions> options)
        {
            var db = client.GetDatabase(options.Value.DatabaseName);
            var containerName = options.Value.Containers["InvoiceAccessTokens"];
            _container = db.GetContainer(containerName);
        }

        public async Task SaveTokenAsync(InvoiceAccessToken record)
        {
            await _container.CreateItemAsync(record, new PartitionKey(record.UserId));
        }

        public async Task<InvoiceAccessToken?> GetTokenAsync(string token)
        {
            var query = new QueryDefinition("SELECT * FROM c WHERE c.token = @token")
                .WithParameter("@token", token);

            using var iterator = _container.GetItemQueryIterator<InvoiceAccessToken>(
                query,
                requestOptions: new QueryRequestOptions
                {
                    PartitionKey = PartitionKey.None // or use fixed key if required
                });

            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                return response.FirstOrDefault();
            }

            return null;
        }

        public async Task InvalidateTokenAsync(string token)
        {
            var existing = await GetTokenAsync(token);
            if (existing == null)
                return;

            await _container.DeleteItemAsync<InvoiceAccessToken>(
                existing.id,
                new PartitionKey(existing.UserId));
        }
    }
}
