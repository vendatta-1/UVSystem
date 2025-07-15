 
using UVS.Common.Application.Messaging;

namespace UVS.Modules.Authentication.Application.Users.GetUser;

public sealed record GetUserQuery(Guid UserId) : IQuery<UserResponse>;
