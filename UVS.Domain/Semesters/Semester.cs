using UVS.Domain.Common;
using UVS.Domain.Students;

namespace UVS.Domain.Semesters;

public sealed class Semester:AuditEntity
{
    public string Name { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public bool IsCurrent { get; private set; }
    public Guid DepartmentId { get; private set; }
    public AcademicLevel AcademicYear { get; private set; }

    private readonly List<SemesterCourse> _semesterCourses = new();
    public IReadOnlyList<SemesterCourse> SemesterCourses => _semesterCourses.AsReadOnly();

    private Semester() {}

    public static Semester Create(string name, DateTime start, DateTime end, Guid deptId, AcademicLevel year, bool isCurrent = false)
    {
        if (start >= end) throw new ArgumentException("Start must be before end");

        return new Semester
        {
            Name = name,
            StartDate = start,
            EndDate = end,
            DepartmentId = deptId,
            AcademicYear = year,
            IsCurrent = isCurrent
        };
    }
}