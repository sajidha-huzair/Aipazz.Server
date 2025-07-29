using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aipazz.Application.Notification.Interfaces;
using Aipazz.Domian;
using Aipazz.Domian.Notification;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;
using NotificationModel = Aipazz.Domian.Notification.Notification;


namespace AIpazz.Infrastructure.Notification
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly Container _container;

        public NotificationRepository(CosmosClient client, IOptions<CosmosDbOptions> options)
        {
            var db = client.GetDatabase(options.Value.DatabaseName);
            var containerName = options.Value.Containers["Notification"];
            _container = db.GetContainer(containerName);
        }

        public async Task<string> CreateNotificationAsync(Aipazz.Domian.Notification.Notification notification)
        {
            await _container.CreateItemAsync(notification, new PartitionKey(notification.UserId));
            return notification.id;
        }

        public async Task<List<NotificationModel>> GetUserNotificationsAsync(string userId, string userEmail)
        {
            var query = new QueryDefinition(@"
        SELECT * FROM c 
        WHERE (c.UserId = @userId 
               OR (IS_NULL(c.UserId) OR c.UserId = '') AND c.RecipientEmail = @userEmail)
        ORDER BY c.CreatedAt DESC")
                .WithParameter("@userId", userId)
                .WithParameter("@userEmail", userEmail);

            var iterator = _container.GetItemQueryIterator<NotificationModel>(
                query,
                requestOptions: new QueryRequestOptions());


            var notifications = new List<NotificationModel>();
            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                notifications.AddRange(response);
            }

            return notifications;
        }


        public async Task<List<Aipazz.Domian.Notification.Notification>> GetUnreadNotificationsAsync(string userId)
        {
            var query = new QueryDefinition("SELECT * FROM c WHERE c.UserId = @userId AND c.IsRead = false ORDER BY c.CreatedAt DESC")
                .WithParameter("@userId", userId);

            var iterator = _container.GetItemQueryIterator<Aipazz.Domian.Notification.Notification>(query);
            var notifications = new List<Aipazz.Domian.Notification.Notification>();

            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                notifications.AddRange(response);
            }

            return notifications;
        }

        public async Task<bool> MarkAsReadAsync(string notificationId, string userId)
        {
            try
            {
                var notification = await _container.ReadItemAsync<Aipazz.Domian.Notification.Notification>(notificationId, new PartitionKey(userId));
                notification.Resource.IsRead = true;
                await _container.ReplaceItemAsync(notification.Resource, notificationId, new PartitionKey(userId));
                return true;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return false;
            }
        }

        public async Task<bool> DeleteNotificationAsync(string notificationId, string userId)
        {
            try
            {
                await _container.DeleteItemAsync<Aipazz.Domian.Notification.Notification>(notificationId, new PartitionKey(userId));
                return true;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return false;
            }
        }

        public async Task<int> GetUnreadCountAsync(string userId)
        {
            var query = new QueryDefinition("SELECT VALUE COUNT(1) FROM c WHERE c.UserId = @userId AND c.IsRead = false")
                .WithParameter("@userId", userId);

            var iterator = _container.GetItemQueryIterator<int>(query);
            
            if (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                return response.FirstOrDefault();
            }

            return 0;
        }

        public async Task AssignNotificationsToUserAsync(string userEmail, string userId)
        {
            var query = new QueryDefinition(@"
        SELECT * FROM c 
        WHERE (IS_NULL(c.UserId) OR c.UserId = '') AND c.RecipientEmail = @userEmail")
                .WithParameter("@userEmail", userEmail);

            var iterator = _container.GetItemQueryIterator<NotificationModel>(
     query,
     requestOptions: new QueryRequestOptions()); // no EnableCrossPartitionQuery here

            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                foreach (var notification in response)
                {
                    // Only patch if UserId is missing
                    if (string.IsNullOrWhiteSpace(notification.UserId))
                    {
                        notification.UserId = userId;
                        await _container.UpsertItemAsync(notification, new PartitionKey(userId));
                    }
                }
            }
        }

    }
}