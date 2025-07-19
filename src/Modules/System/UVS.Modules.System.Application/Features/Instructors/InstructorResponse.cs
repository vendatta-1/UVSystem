namespace UVS.Modules.System.Application.Features.Instructors;

public sealed class InstructorResponse (string fullName, string  email, Guid  deptId)
{
    public string FullName { get; init; } = fullName;
    public string Email { get; init; } = email;
    public Guid DepartmentId { get; init; } = deptId;
}