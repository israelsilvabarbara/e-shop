using System.Security.Claims;

namespace Shared.Keycloak.Services
{
    public interface IUserService
    {
        string? GetUserId(ClaimsPrincipal user);
        bool IsAdmin(ClaimsPrincipal user);
    }
}