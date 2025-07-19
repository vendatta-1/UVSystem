using UVS.Common.Application.Messaging;

namespace UVS.Modules.Authentication.Application.Identity.GetRoleId;

public sealed record GetRoleIdCommand(string RoleName):ICommand<string>;