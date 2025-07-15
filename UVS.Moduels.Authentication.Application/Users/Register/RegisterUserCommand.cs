using UVS.Authentication.Domain.Users;
using UVS.Common.Application.Messaging;

namespace UVS.Modules.Authentication.Application.Users.Register;

public sealed record RegisterUserCommand(string Email, string Password, string FirstName, string LastName, Role Role) :
    ICommand<Guid>
{
    
}