using UVS.Common.Application.Messaging;

namespace UVS.Modules.Authentication.Application.Users.Login;

public sealed record LoginCommand(string Username, string Password):ICommand<string>;