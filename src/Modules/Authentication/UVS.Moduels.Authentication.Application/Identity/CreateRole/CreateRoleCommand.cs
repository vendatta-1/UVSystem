using UVS.Common.Application.Messaging;

namespace UVS.Modules.Authentication.Application.Identity.CreateRole;

public sealed record CreateRoleCommand(string Name, string Description) : ICommand<string>
{
 
}