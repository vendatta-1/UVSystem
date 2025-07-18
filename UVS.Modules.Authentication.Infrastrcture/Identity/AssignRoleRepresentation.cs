using Newtonsoft.Json;

namespace UVS.Modules.Authentication.Infrastructure.Identity;

internal sealed record AssignRoleRepresentation
{
    [JsonProperty("id")]
    public Guid Id { get; init; }
    [JsonProperty("name")]
    public string Name { get; init; }
}