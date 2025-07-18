using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using UVS.Modules.Authentication.Application.Abstractions.Identity;

namespace UVS.Modules.Authentication.Infrastructure.Identity;

internal sealed class KeyCloakClient(HttpClient httpClient, IOptions<KeyCloakOptions> options)
{
    private readonly KeyCloakOptions keyOptions=options.Value;

    internal async Task<string> GetRoleIdAsync(string roleName, CancellationToken cancellationToken)
    {
        
        HttpResponseMessage response = await httpClient.GetAsync($"roles/{roleName}", cancellationToken);
        
        response.EnsureSuccessStatusCode();
        
        var result = await response.Content.ReadFromJsonAsync<RoleIdRepresentation>(cancellationToken);
        
        var id = result is null?throw new ApplicationException("Could not get role id"):result.Id;

        return result.Id;
    }

    internal async Task AssignRoleAsync( string roleName , string identityId, CancellationToken cancellationToken = default)
    {
        var roleId = await  GetRoleIdAsync(roleName, cancellationToken);
        var roleRepresentation = new AssignRoleRepresentation()
        {
            Id = Guid.Parse(roleId),
            Name = roleName
        };
        var requestContent = new List<AssignRoleRepresentation>()
        {
            roleRepresentation
        };
        HttpResponseMessage response = await httpClient.PostAsJsonAsync($"users/{identityId}/role-mappings/realm",requestContent, cancellationToken);
        
        response.EnsureSuccessStatusCode();
        
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

    internal async Task<string> LoginAsync(LoginRepresentation model, CancellationToken cancellationToken = default)
    {
        var authRequestParameters = new KeyValuePair<string, string>[]
        {
            new("client_id", keyOptions.PublicClientId),
            new("username", model.Username),
            new("password", model.Password),
            new("scope", "email openid"),
            new("grant_type", "password"),
        };
        
        using var authRequestContent = new FormUrlEncodedContent(authRequestParameters);
        
        using var authRequestMessage= new HttpRequestMessage(HttpMethod.Post, new Uri(keyOptions.TokenUrl));

        authRequestMessage.Content = authRequestContent;
        HttpResponseMessage httpResponseMessage= await httpClient.SendAsync(authRequestMessage, cancellationToken);
        
        httpResponseMessage.EnsureSuccessStatusCode();

        var token = await httpResponseMessage.Content.ReadFromJsonAsync<AccessToken>(cancellationToken);

        return token?.Token?? throw new ApplicationException($"this user couldn't be authenticated");

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
    [JsonProperty("id")] 
    public string Id { get; set; } = null!;
}

internal class AccessToken
{
    [JsonPropertyName("access_token")]
    public string Token { get; set; } = null!;
}