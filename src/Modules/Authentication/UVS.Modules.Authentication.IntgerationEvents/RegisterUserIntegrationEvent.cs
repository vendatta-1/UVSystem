 
using UVS.Common.Application.EventBus;
namespace UVS.Modules.Authentication.IntegrationEvents;
public sealed class RegisterUserIntegrationEvent
    (   Guid id, 
        DateTime occurredOnUtc,
        string firstName,
        string lastName,
        string email,
        Guid userId)
    : IntegrationEvent(id, occurredOnUtc)
{
    public string FirstName{get; init;} = firstName;
    public string LastName{get; init;} = lastName;
    public string Email{get; init;} = email;
    public Guid UserId{get; init;} =  userId;
}