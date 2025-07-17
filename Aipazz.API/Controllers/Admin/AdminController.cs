using System.Security.Claims;
using Aipazz.Application.Admin.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Aipazz.API.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet("is-admin")]
        [Authorize]
        public IActionResult IsAdmin()
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == "emails")?.Value;

            if (string.IsNullOrEmpty(email))
                return Unauthorized("Email not found in token.");

            bool isAdmin = _adminService.IsAdminEmail(email);

            return Ok(new { isAdmin });
        }
    }
}

