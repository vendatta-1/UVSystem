using UVS.Domain.Common;

namespace UVS.Domain.Courses;

public sealed class UpdateCourseDomainEvent(Guid courseId) : DomainEvent
{
    public Guid CourseId { get; init; } = courseId;
}