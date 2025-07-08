using Microsoft.Extensions.Logging;
using UVS.Common.Application.Clock;
using UVS.Domain.Courses;
using UVS.Modules.System.Infrastructure.Data;

namespace UVS.Modules.System.Infrastructure.Repositories;

internal sealed class CourseRepository(UVSDbContext context, ILogger<CourseRepository> logger, IDateTimeProvider dateTimeProvider) :
    Repository<Course>(context, logger, dateTimeProvider),ICourseRepository
{
    
}