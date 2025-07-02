using UVS.Domain.Common;
using UVS.Domain.Courses;
using UVS.Domain.Semesters;

namespace UVS.Domain.Departments;

public sealed class Department:AuditEntity
{
    public string Name { get; private set; }
    public int MaxCreditHoursPerSemester { get; private set; }

    private readonly List<Course> _courses = new();
    public IReadOnlyList<Course> Courses => _courses.AsReadOnly();

    private readonly List<Semester> _semesters = new();
    public IReadOnlyList<Semester> Semesters => _semesters.AsReadOnly();

    public Department() {}

    public static Department Create(string name, Guid headId, int maxCreditHours)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name is required");
        if (maxCreditHours < 1 || maxCreditHours > 30) throw new ArgumentOutOfRangeException("Credit hours limit invalid");

        return new Department
        {
            Name = name,
            MaxCreditHoursPerSemester = maxCreditHours,
            HeadOfDepartmentId = headId
        };
    }

    public Guid HeadOfDepartmentId { get; private set; }
}