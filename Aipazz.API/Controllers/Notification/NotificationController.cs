using System.Security.Claims;
using Aipazz.Application.Notification.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aipazz.API.Controllers.Notification
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationRepository _repository;

        public NotificationController(INotificationRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateNotification([FromBody] Aipazz.Domian.Notification.Notification request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized("User ID not found in token.");

            // Set the server-controlled fields
            request.id = Guid.NewGuid().ToString();
            request.UserId = userId; // Override with token userId for security
            request.IsRead = false; // Always start as unread
            request.CreatedAt = DateTime.UtcNow; // Server timestamp

            // Set default CreatedBy if not provided
            if (string.IsNullOrWhiteSpace(request.CreatedBy))
                request.CreatedBy = "System";

            var notificationId = await _repository.CreateNotificationAsync(request);

            return Ok(request);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserNotifications()
        {
            string? userId = User.Claims
                .FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")
                ?.Value;

            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized("User ID not found in token.");

            var notifications = await _repository.GetUserNotificationsAsync(userId);
            return Ok(notifications);
        }

        [HttpGet("unread")]
        public async Task<IActionResult> GetUnreadNotifications()
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized("User ID not found in token.");

            var notifications = await _repository.GetUnreadNotificationsAsync(userId);
            return Ok(notifications);
        }

        [HttpGet("count")]
        public async Task<IActionResult> GetUnreadCount()
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized("User ID not found in token.");

            var count = await _repository.GetUnreadCountAsync(userId);
            return Ok(new { count });
        }

        [HttpPut("{id}/read")]
        public async Task<IActionResult> MarkAsRead(string id)
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized("User ID not found in token.");

            var success = await _repository.MarkAsReadAsync(id, userId);
            return success ? Ok() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotification(string id)
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized("User ID not found in token.");

            var success = await _repository.DeleteNotificationAsync(id, userId);
            return success ? NoContent() : NotFound();
        }
    }
}
