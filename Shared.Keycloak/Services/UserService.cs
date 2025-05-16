using System.Linq;
using System.Security.Claims;
using System.Text.Json;

namespace Shared.Keycloak.Services
{

    public class UserAuthService : IUserService
    {
        public string? GetUserId(ClaimsPrincipal user)
        {
            return user.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
        }

        public bool IsAdmin(ClaimsPrincipal user)
        {
            return user.Claims
                       .Where(c => c.Type == "realm_access")
                       .SelectMany(c => JsonSerializer.Deserialize<RealmAccess>(c.Value)?.Roles ?? [])
                       .Contains("administrator");
        }

        private class RealmAccess
        {
            public string[]? Roles { get; set; }
        }
    }
}