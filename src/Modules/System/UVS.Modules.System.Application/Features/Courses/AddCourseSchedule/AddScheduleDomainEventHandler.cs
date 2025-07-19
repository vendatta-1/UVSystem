using MediatR;
using UVS.Common.Application.Messaging;
using UVS.Common.Domain;
using UVS.Domain.Courses;

namespace UVS.Modules.System.Application.Features.Courses.AddCourseSchedule;

internal sealed class AddScheduleDomainEventHandler(ISender sender) :
    DomainEventHandler<UpdateCourseDomainEvent>
{
    public override Task Handle(UpdateCourseDomainEvent domainEvent, CancellationToken cancellationToken)
    { 
        Console.WriteLine($"{nameof(AddScheduleDomainEventHandler)}.{nameof(Handle)}");
        return Task.CompletedTask;
    }
 
}