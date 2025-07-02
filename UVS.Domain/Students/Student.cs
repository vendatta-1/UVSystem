using System.ComponentModel.DataAnnotations;
using UVS.Domain.Common;
using UVS.Domain.Courses;
using UVS.Domain.Enrollments;

namespace UVS.Domain.Students;

public sealed class Student :AuditEntity
{
    public string FullName { get; private set; }
    public string NationalId { get; private set; }
    public string Email { get; private set; }
    public string Phone { get; private set; }
    public DateTime DateOfBirth { get; private set; }
    public string Gender { get; private set; }
    public Guid DepartmentId { get; private set; }
    public AcademicLevel Level { get; private set; } = AcademicLevel.Freshman;

    private readonly List<Enrollment> _enrollments = new();
    public IReadOnlyList<Enrollment> Enrollments => _enrollments.AsReadOnly();

    public Student() {}

    public static Student Create(string fullName, string nationalId, string email, string phone, DateTime dob, string gender, Guid deptId)
    {
        return new Student()
        {
            FullName = fullName,
            NationalId = nationalId,
            Email = email,
            Phone = phone,
            DateOfBirth = dob,
            Gender = gender,
            DepartmentId = deptId
        };

    }

    public void EnrollInCourse(Course course, DateTime enrollmentDate)
    {
        if (_enrollments.Any(e => e.CourseId == course.Id))
            throw new InvalidOperationException("Student already enrolled in this course.");

        if (!course.IsAvailable)
            throw new InvalidOperationException("Course is not currently available.");

        _enrollments.Add(Enrollment.Create(Id, course.Id, enrollmentDate));
    }

    public double CalculateGpa()
    {
        var graded = _enrollments.Where(e => e.Grade.HasValue);
        int totalCredits = graded.Sum(e => e.Course?.CreditHours ?? 0);
        double totalPoints = graded.Sum(e => (e.Course?.CreditHours ?? 0) * e.GetGradePoint());

        return totalCredits == 0 ? 0 : totalPoints / totalCredits;
    }

}