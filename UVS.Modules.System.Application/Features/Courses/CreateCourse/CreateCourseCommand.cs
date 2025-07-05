using UVS.Common.Application.Messaging;

namespace UVS.Modules.System.Application.Features.Courses.CreateCourse;

public sealed class CreateCourseCommand:ICommand<Guid>
{
    public string Code { get;   init; } = null!;
    public string Name { get;   init; } = null!;
    public string? Description { get;   init; }
    public int CreditHours { get;   init; }
    public Guid DepartmentId { get;   init; }
    public Guid SemesterId { get;   init; }
    public Guid? InstructorId { get;   init; }
    
}