using Microsoft.Extensions.Logging;
using UVS.Common.Application.Clock;
using UVS.Domain.Semesters;
using UVS.Modules.System.Infrastructure.Data;

namespace UVS.Modules.System.Infrastructure.Repositories;

internal sealed class SemesterRepository(UVSDbContext dbContext,ILogger<SemesterRepository> logger, IDateTimeProvider dateTimeProvider) :
    Repository<Semester>(dbContext,logger, dateTimeProvider),ISemesterRepository
{
    
}