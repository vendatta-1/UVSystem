using UVS.Domain.Courses;

namespace UVS.Domain.Semesters;

public class SemesterCourse
{
    public Guid SemesterId { get; private set; }
    public Guid CourseId { get; private set; }

    public Semester Semester { get; private set; } = null!;
    public Course Course { get; private set; } = null!;

    private SemesterCourse() {}

    public static SemesterCourse Create(Guid semesterId, Guid courseId) =>
        new() { SemesterId = semesterId, CourseId = courseId };
}