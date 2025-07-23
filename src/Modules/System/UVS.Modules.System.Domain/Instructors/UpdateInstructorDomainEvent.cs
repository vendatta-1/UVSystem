 
using UVS.Common.Domain;

namespace UVS.Domain.Instructors;

public sealed class UpdateInstructorDomainEvent(Guid instructorId):DomainEvent
{
    public Guid InstructorId { get; init; } =  instructorId; 
}