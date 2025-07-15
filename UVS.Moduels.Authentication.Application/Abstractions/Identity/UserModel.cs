using UVS.Authentication.Domain.Users;

namespace UVS.Modules.Authentication.Application.Abstractions.Identity;

public sealed record UserModel(string Email, string Password, string FirstName, string LastName);
