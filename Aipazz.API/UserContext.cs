using Aipazz.Application.Common;
using Aipazz.Application.Common.Aipazz.Application.Common;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

public class UserContext : IUserContext
{
    private readonly IHttpContextAccessor _http;

    public UserContext(IHttpContextAccessor http) => _http = http;

    public string UserId =>
        _http.HttpContext?.User.FindFirst("oid")?.Value
        ?? _http.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
        ?? string.Empty;

    public string FullName =>
        _http.HttpContext?.User.FindFirst("name")?.Value
        ?? $"{_http.HttpContext?.User.FindFirst("given_name")?.Value} {_http.HttpContext?.User.FindFirst("family_name")?.Value}".Trim();

    public string Email =>
        _http.HttpContext?.User?.FindFirstValue(ClaimTypes.Email)
        ?? _http.HttpContext?.User?.FindFirstValue("emails")
        ?? _http.HttpContext?.User?.FindFirstValue("preferred_username")
        ?? throw new UnauthorizedAccessException("Email not found in user claims");

}
