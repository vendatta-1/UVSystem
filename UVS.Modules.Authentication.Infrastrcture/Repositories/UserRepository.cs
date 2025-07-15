using Microsoft.Extensions.Logging;
using UVS.Authentication.Domain.Users;
using UVS.Authentication.Infrastructure.Data;
using UVS.Common.Application.Clock;

namespace UVS.Authentication.Infrastructure.Repositories;

internal sealed class UserRepository(
    AuthDbContext dbContext,
    ILogger<Repository<User>> logger,
    IDateTimeProvider dateTimeProvider) :
    Repository<User>(dbContext, logger, dateTimeProvider), IUserRepository
{
    public override Task<Guid> CreateAsync(User entity)
    {
        foreach (var role in entity.Roles)
        {
            dbContext.Attach(role);
        }
        return base.CreateAsync(entity);
    }

}