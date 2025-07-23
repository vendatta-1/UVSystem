using UVS.Common.Domain;

namespace UVS.Domain.Courses;

public sealed class DeactivateCourseDomainEvent(Guid courseId):DomainEvent
{
    public Guid CourseId { get; init; } = courseId;
}