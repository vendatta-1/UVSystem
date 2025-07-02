using UVS.Domain.Common;

namespace UVS.Domain.Courses;

public sealed class ChangeCourseCodeDomainEvent(Guid courseId, string code) : DomainEvent
{
    public Guid CourseId { get; init; } = courseId;
    public string Code { get; init; } = code;
}