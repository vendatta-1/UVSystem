using Microsoft.Extensions.Logging;
using UVS.Domain.Courses;
using UVS.Modules.System.Infrastructure.Data;

namespace UVS.Modules.System.Infrastructure.Repositories;

internal sealed class CourseRepository(UVSDbContext context, ILogger<CourseRepository> logger):
    Repository<Course>(context, logger),ICourseRepository
{
    
}