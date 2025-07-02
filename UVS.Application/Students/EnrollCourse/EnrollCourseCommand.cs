using UVS.Application.Messaging;

namespace UVS.Application.Students.EnrollCourse;

public record EnrollCourseCommand(Guid CourseId, Guid StudentId):ICommand
{ 
    
}