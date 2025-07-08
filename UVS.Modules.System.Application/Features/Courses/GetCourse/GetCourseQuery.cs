using UVS.Common.Application.Messaging;

namespace UVS.Modules.System.Application.Features.Courses.GetCourse;

public sealed record GetCourseQuery(Guid CourseId) : IQuery<CourseResponse>
{
    
}