using UVS.Domain.Common;

namespace UVS.Domain.Instructors;

public sealed class UpdateInstructorDomainEvent(Guid instructorId):DomainEvent
{
    public Guid InstructorId { get; init; } =  instructorId; 
}