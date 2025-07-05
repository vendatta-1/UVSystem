using Microsoft.Extensions.Logging;
using UVS.Domain.Semesters;
using UVS.Modules.System.Infrastructure.Data;

namespace UVS.Modules.System.Infrastructure.Repositories;

internal sealed class SemesterRepository(UVSDbContext dbContext,ILogger<SemesterRepository> logger):
    Repository<Semester>(dbContext,logger),ISemesterRepository
{
    
}