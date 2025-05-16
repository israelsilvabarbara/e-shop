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
            Console.WriteLine("################################################");
            Console.WriteLine("################################################");
            Console.WriteLine("################################################");
            Console.WriteLine("################################################");
            Console.WriteLine("################################################");
            Console.WriteLine("################################################");
            Console.WriteLine("################################################");
            Console.WriteLine("################################################");
            Console.WriteLine("################################################");
            
            var identity = principal.Identities.FirstOrDefault(i => i.IsAuthenticated);

            if (identity != null)
            {
                var realmAccessClaim = principal.FindFirst("realm_access")?.Value;
                Console.WriteLine("DEBUG: realmAccessClaim: " + realmAccessClaim);
                if (!string.IsNullOrEmpty(realmAccessClaim))
                {
                    try
                    {
                        var realmAccess = JsonSerializer.Deserialize<RealmAccess>(realmAccessClaim);
                        Console.WriteLine("DEBUG: realm_access: " + JsonSerializer.Serialize(realmAccess));
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
                }else
                {
                    Console.WriteLine("DEBUG: ERROR: realm_access claim not found");
                }
            }else
            {
                Console.WriteLine("DEBUG: ERROR: Identity not found");
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