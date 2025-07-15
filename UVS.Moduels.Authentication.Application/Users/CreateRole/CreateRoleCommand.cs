using UVS.Common.Application.Messaging;

namespace UVS.Modules.Authentication.Application.Users.CreateRole;

public sealed record CreateRoleCommand(string Name, string Description) : ICommand<string>
{
 
}