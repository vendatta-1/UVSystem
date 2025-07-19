namespace UVS.Modules.Authentication.Infrastructure.Identity;

internal sealed record UserRepresentation(
    string Username,
    string Email,
    string FirstName,
    string LastName,
    bool EmailVerified,
    bool Enabled,
    CredentialRepresentation[] Credentials);
