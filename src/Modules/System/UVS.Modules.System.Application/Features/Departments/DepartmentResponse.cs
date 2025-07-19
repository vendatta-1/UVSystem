namespace UVS.Modules.System.Application.Features.Departments;

public class DepartmentResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public int MaxCreditHoursPerSemester { get; init; }
    public int StudentsCount { get; init; }
    public int CoursesCount { get; init; }
}