using UVS.Common.Application.EventBus;

namespace UVS.Modules.System.Integration;

public sealed class CreateStudentIntegrationEvent : IntegrationEvent
{
    public CreateStudentIntegrationEvent(
        Guid id,
        DateTime occurredOnUtc,
        Guid studentId,
        string firstName,
        string lastName,
        string email
        ) : 
        base(id, occurredOnUtc)
    {
        
        FirstName = firstName;
        LastName = lastName;
        Email = email; 
        StudentId = studentId;
    }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; } 
    public Guid StudentId { get; private set; }
    
}