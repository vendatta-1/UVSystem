using Microsoft.Extensions.Logging;
using UVS.Common.Application.Clock;
using UVS.Domain.Departments;
using UVS.Modules.System.Infrastructure.Data;

namespace UVS.Modules.System.Infrastructure.Repositories;

internal sealed class DepartmentRepository(UVSDbContext context,ILogger<DepartmentRepository> logger, IDateTimeProvider dateTimeProvider) :
    Repository<Department>(context, logger, dateTimeProvider), IDepartmentRepository
{
    
}