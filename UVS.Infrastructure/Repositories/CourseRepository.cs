using Microsoft.Extensions.Logging;
using UVS.Domain.Courses;
using UVS.Infrastructure.Data;

namespace UVS.Infrastructure.Repositories;

internal sealed class CourseRepository(UVSDbContext context, ILogger<CourseRepository> logger):
    Repository<Course>(context, logger),ICourseRepository
{
    
}