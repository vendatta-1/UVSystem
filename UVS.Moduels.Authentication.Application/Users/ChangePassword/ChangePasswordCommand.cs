using UVS.Common.Application.Messaging;

namespace UVS.Modules.Authentication.Application.Users.ChangePassword;

public sealed record ChangePasswordCommand(string Username, string OldPassword, string NewPassword):ICommand;