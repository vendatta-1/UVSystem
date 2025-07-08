using UVS.Common.Domain;
 
using UVS.Domain.Enrollments;
using UVS.Domain.Schedules;
using UVS.Domain.Semesters;

namespace UVS.Domain.Courses;

public sealed class Course:AuditEntity
{
    public string Code { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }
    public int CreditHours { get; private set; }
    public Guid DepartmentId { get; private set; }
    public Guid? InstructorId { get; private set; }

    private readonly List<Schedule> _schedules = new();
    public IReadOnlyList<Schedule> Schedules => _schedules.AsReadOnly();

    private readonly List<Enrollment> _enrollments = new();
    public IReadOnlyList<Enrollment> Enrollments => _enrollments.AsReadOnly();

    private readonly List<SemesterCourse> _semesterCourses = new();
    public IReadOnlyList<SemesterCourse> SemesterCourses => _semesterCourses.AsReadOnly();

    public bool IsAvailable => true;  

    private Course() {}

    public static Course Create(string code, string name, int creditHours, Guid departmentId, Guid? instructorId, string? desc)
    {
        if (string.IsNullOrWhiteSpace(code)) throw new ArgumentException("Course code is required");
        if (creditHours < 1 || creditHours > 6) throw new ArgumentOutOfRangeException(nameof(creditHours));

        var course =new Course
        {
            Code = code,
            Name = name,
            CreditHours = creditHours,
            DepartmentId = departmentId,
            InstructorId = instructorId,
            Description = desc
        };
        course.Raise(new CreateCourseDomainEvent(course.Id));
        return course;
        
    }

    public void UpdateCourseCode(string code)
    {
        code = code ?? throw new ArgumentNullException(nameof(code));
        if (string.IsNullOrWhiteSpace(code)) throw new ArgumentException("Course code is required");
        Code = code;
        Raise(new ChangeCourseCodeDomainEvent(Id, code));
    }

    public void AddSchedule(Schedule schedule)
    {
        if  (schedule == null) throw new ArgumentNullException(nameof(schedule));
        if (_schedules.Contains(schedule)) throw new ArgumentException("Schedule already exists");
        if(schedule.CourseId !=Id) throw new ArgumentException("Course id is not valid");
        _schedules.Add(schedule);
        Raise(new UpdateCourseDomainEvent(Id));
    }
    
}