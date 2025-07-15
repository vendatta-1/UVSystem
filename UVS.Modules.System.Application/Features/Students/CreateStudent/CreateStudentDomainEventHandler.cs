using MediatR;
using UVS.Common.Application.EventBus;
using UVS.Common.Application.Messaging;
using UVS.Common.Domain;
using UVS.Domain.Students;
using UVS.Modules.System.Application.Features.Students.GetStudent; 
using  UVS.Modules.System.Integration;
 
namespace UVS.Modules.System.Application.Features.Students.CreateStudent;

internal sealed class CreateStudentDomainEventHandler(
    IEventBus eventBus, 
    ISender sender) : DomainEventHandler<CreateStudentDomainEvent>
{
    public override async Task Handle(CreateStudentDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        Result<StudentResponse> result = await sender.Send(new GetStudentQuery(domainEvent.StudentId), cancellationToken);

        await eventBus.PublishAsync(new CreateStudentIntegrationEvent(
            domainEvent.Id,
            domainEvent.OccurredOnUtc,
            domainEvent.StudentId,
            result.Value.FirstName,
            result.Value.LastName,
            result.Value.Email 
        ));
    }
}