namespace UVS.Modules.Authentication.Application.Abstractions.Identity;

public sealed record ChangePasswordModel(string Username, string CurrentPassword, string NewPassword);
