using System.Net.Http.Json;
using Newtonsoft.Json;

namespace UVS.Authentication.Infrastructure.Identity;

internal sealed class KeyCloakClient(HttpClient httpClient)
{

    internal async Task<string> GetRoleIdAsync(string roleName, CancellationToken cancellationToken)
    {
        
        HttpResponseMessage response = await httpClient.GetAsync($"api/roles/{roleName}", cancellationToken);
        
        response.EnsureSuccessStatusCode();
        
        var result =await response.Content.ReadFromJsonAsync<RoleIdRepresentation>(cancellationToken);
        var id = result is null?throw new ApplicationException("Could not get role id"):result.Id;

        return result.Id;
    }
    internal async Task<string> CreateRoleAsync(CreateRoleRepresentation role, CancellationToken cancellationToken = default)
    {
        HttpResponseMessage response = await httpClient.PostAsJsonAsync(
            $"roles",
            role,
            cancellationToken
        );

        response.EnsureSuccessStatusCode();
 
        string? location = response.Headers.Location?.ToString();
        if (string.IsNullOrEmpty(location))
            throw new InvalidOperationException("Role creation failed: Location header missing.");
 
        string roleName = location.Substring(location.LastIndexOf("/") + 1);
        return roleName;
    }
    internal async Task<string> RegisterUserAsync(UserRepresentation user, CancellationToken cancellationToken = default)
    {
        HttpResponseMessage httpResponseMessage = await httpClient.PostAsJsonAsync(
            "users",
            user,
            cancellationToken);

        httpResponseMessage.EnsureSuccessStatusCode();

        return ExtractIdentityIdFromLocationHeader(httpResponseMessage);
    }

    private static string ExtractIdentityIdFromLocationHeader(
        HttpResponseMessage httpResponseMessage)
    {
        const string usersSegmentName = "users/";

        string? locationHeader = httpResponseMessage.Headers.Location?.PathAndQuery;

        if (locationHeader is null)
        {
            throw new InvalidOperationException("Location header is null");
        }

        int userSegmentValueIndex = locationHeader.IndexOf(
            usersSegmentName,
            StringComparison.InvariantCultureIgnoreCase);

        string identityId = locationHeader.Substring(userSegmentValueIndex + usersSegmentName.Length);

        return identityId;
    }
    
}
internal class RoleIdRepresentation
{
    [JsonProperty("id")] public string Id { get; set; } = null!;
}
