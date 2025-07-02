using UVS.Domain.Common;

namespace UVS.Domain.Students;

public sealed class CreateStudentDomainEvent(Guid studentId) : DomainEvent
{
    public Guid StudentId { get; init; } = studentId;
}