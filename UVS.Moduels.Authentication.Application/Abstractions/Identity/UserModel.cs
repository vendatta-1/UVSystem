using UVS.Authentication.Domain.Users;

namespace UVS.Authentication.Application.Abstractions.Identity;

public sealed record UserModel(string Email, string Password, string FirstName, string LastName, Role Role);
