using IdentityServer4.Models;

namespace Identity;

internal static class IdentityServerConfig
{
    public static IReadOnlyCollection<ApiResource> GetResources()
    {
        return new[]
        {
            new ApiResource("employee.api", "employee API")
            {
                Scopes = new[]
                {
                    "employee.read",
                    "employee.write",
                    "employee.update",
                    "employee.delete"
                }
            }
        };
    }

    public static IReadOnlyCollection<ApiScope> GetScopes()
    {
        return new[]
        {
            new ApiScope("employee.read", "Read"),
            new ApiScope("employee.write", "Write"),
            new ApiScope("employee.update", "Update"),
            new ApiScope("employee.delete", "Delete")
        };
    }

    public static IReadOnlyCollection<Client> GetClients()
    {
        return new[]
        {
            new Client
            {
                ClientId = "employee.client",
                ClientName = "Employee Client",
                ClientSecrets = new[] {new Secret("4d740daf-1861-401a-9615-d6dea24fc794".Sha256())},
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                AllowedScopes = {"employee.read"}
            }
        };
    }
}