namespace UVS.Modules.Authentication.Application.Abstractions.Identity;

public sealed record ChangePasswordModel(Guid userId, string currentPassword, string newPassword) 
{
    public Guid UserId { get; init; } = userId;
    public string CurrentPassword { get; init; } = currentPassword;
    public string NewPassword { get; init; } = newPassword;
}