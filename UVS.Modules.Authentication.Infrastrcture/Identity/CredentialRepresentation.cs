namespace UVS.Modules.Authentication.Infrastructure.Identity;

internal sealed record CredentialRepresentation(string Type, string Value, bool Temporary);