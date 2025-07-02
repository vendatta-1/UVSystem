using Microsoft.Extensions.Logging;
using UVS.Domain.Departments;
using UVS.Infrastructure.Data;

namespace UVS.Infrastructure.Repositories;

internal sealed class DepartmentRepository(UVSDbContext context,ILogger<DepartmentRepository> logger) :
    Repository<Department>(context,logger), IDepartmentRepository
{
    
}