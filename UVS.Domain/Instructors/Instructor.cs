using UVS.Domain.Common;
using UVS.Domain.Courses;

namespace UVS.Domain.Instructors;

public sealed class Instructor : AuditEntity
{

    public string FullName { get; private set; } = null!;
    public string Email { get; private set; } = null!;
    public Guid DepartmentId { get; private set; }

    private readonly List<Course> _courses = new();
    
    public IReadOnlyList<Course> Courses => _courses.AsReadOnly();

    public List<OfficeHour> OfficeHours { get; private set; } = new();

    public Instructor()
    {
    }

    public static Instructor Create(string name, string email, Guid deptId)
    {
        return new Instructor
        {
            FullName = name,
            Email = email,
            DepartmentId = deptId
        };
    }

    public void AddCourse(Course course)
    {
        if (_courses.Contains(course))
        {
            throw new ArgumentException("Course already exists");
        }
        _courses.Add(course);
    }

    public void RemoveCourse(Guid courseId)
    {
        if (_courses.Any(c => c.Id == courseId))
        {
            _courses.RemoveAll(c => c.Id == courseId);
        }
        throw new ArgumentException("Course doesn't exist");
    }

    public void AddOfficeHour(OfficeHour officeHour)
    {
        if (OfficeHours.Contains(officeHour) )
        {
            return;
        }

        if (OfficeHours.Count(of => of.Day == officeHour.Day) >= 2)
        {
            return;
        }
        OfficeHours.Add(officeHour);
    }
    
}