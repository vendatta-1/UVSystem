using UVS.Common.Domain;
using UVS.Domain.Common;
using UVS.Domain.Courses;

namespace UVS.Domain.Enrollments;
public sealed class Enrollment : AuditEntity
{
    public Guid StudentId { get; private set; }
    public Guid CourseId { get; private set; }
    public Course? Course { get; private set; }
    public DateTime EnrollmentDate { get; private set; }
    public double? Grade { get; private set; }
    public bool IsWithdrawn { get; private set; }
    public DateTime? WithdrawalDate { get; private set; }

    public bool IsActive => Grade == null && !IsWithdrawn;

    public Enrollment() { }

    public static Enrollment Create(Guid studentId, Guid courseId, DateTime date) =>
        new()
        {
            StudentId = studentId,
            CourseId = courseId,
            EnrollmentDate = date,
            IsWithdrawn = false
        };

    public void Withdraw(DateTime now)
    {
        if ((now - EnrollmentDate).TotalDays > 14)
            throw new InvalidOperationException("Cannot withdraw after 14 days of enrollment.");

        if (!IsActive)
            throw new InvalidOperationException("Cannot withdraw from a completed or already withdrawn course.");

        IsWithdrawn = true;
        WithdrawalDate = now;
    }

    public double GetGradePoint()
        => (Grade.HasValue && !IsWithdrawn) ? Grade.Value.ToGpaPoint() : 0.0;
}