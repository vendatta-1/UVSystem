namespace UVS.Modules.System.Application.Features.Courses;

public sealed record CourseResponse
{
    public string Code { get; init; } = null!;
    public string Name { get; init; } = null!;
    public string? Description { get; init; }
    public int CreditHours { get; init; }
    public Guid DepartmentId { get; init; }
    public Guid? InstructorId { get; init; }
    public Guid SemesterId { get; init; }
}