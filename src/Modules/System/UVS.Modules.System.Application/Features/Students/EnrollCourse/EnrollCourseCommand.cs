
using UVS.Common.Application.Messaging;

namespace UVS.Modules.System.Application.Features.Students.EnrollCourse;

public record EnrollCourseCommand(Guid CourseId, Guid StudentId):ICommand
{ 
    
}