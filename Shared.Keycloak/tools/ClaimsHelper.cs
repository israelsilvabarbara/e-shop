using System.Linq;
using System.Security.Claims;
using System.Text.Json;

namespace Shared.Keycloak.Tools
{

    public static class ClaimsHelper
    {
        public static string? GetUserId(ClaimsPrincipal user)
        {
            return user.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
        }
    }
}