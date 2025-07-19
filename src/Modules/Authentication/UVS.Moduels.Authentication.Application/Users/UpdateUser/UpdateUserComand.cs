 

using UVS.Common.Application.Messaging;

namespace UVS.Modules.Authentication.Application.Users.UpdateUser;

public sealed record UpdateUserCommand(Guid UserId, string FirstName, string LastName) : ICommand;
