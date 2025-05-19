using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Shared.Keycloak.Services
{
    public class KeycloakClaimsTransformer : IClaimsTransformation
    {
        public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            var identity = principal.Identities.FirstOrDefault(i => i.IsAuthenticated);

            if (identity != null)
            {
                var realmAccessClaim = principal.FindFirst("realm_access")?.Value;

                if (!string.IsNullOrEmpty(realmAccessClaim))
                {
                    try
                    {
                        var realmAccess = JsonSerializer.Deserialize<RealmAccess>(realmAccessClaim);
                        if (realmAccess?.Roles != null)
                        {
                            foreach (var role in realmAccess.Roles)
                            {
                                Console.WriteLine($"Adding role: {role}");
                                identity.AddClaim(new Claim(ClaimTypes.Role, role));
                            }
                        }
                    }
                    catch (JsonException ex)
                    {
                        // Handle potential JSON parsing errors
                        Console.WriteLine($"Error parsing realm_access claim: {ex.Message}");
                    }
                }
                
            }

            return Task.FromResult(principal);
        }

        private class RealmAccess
        {
            [JsonPropertyName("roles")]
            public string[]? Roles { get; set; }
        }
    }
}