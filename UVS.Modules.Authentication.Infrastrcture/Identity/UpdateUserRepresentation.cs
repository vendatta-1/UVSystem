namespace UVS.Authentication.Infrastructure.Identity;
internal sealed record UpdateUserRepresentation(
    string Id,
    string Username,
    string Email,
    string FirstName,
    string LastName,
    bool Enabled,
    bool EmailVerified
);
