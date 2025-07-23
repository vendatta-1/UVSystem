using UVS.Common.Domain;
 

namespace UVS.Domain.Instructors;

public sealed class InstructorAddCourseDomainEvent(Guid courseId) : DomainEvent
{
    public Guid CourseId { get; init; } = courseId;
}