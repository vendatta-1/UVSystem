using UVS.Common.Application.EventBus;

namespace UVS.Modules.System.Integration;

public sealed class CreateInstructorIntegrationEvent:IntegrationEvent
{
    public CreateInstructorIntegrationEvent(
        Guid id,
        DateTime occurredOnUtc,
        string firstName,
        string lastName,
        string email) :
        base(id, occurredOnUtc)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
    }
    public string FirstName { get; init; }  
    public string LastName { get; init; }
    public string Email { get; init; }
    
}