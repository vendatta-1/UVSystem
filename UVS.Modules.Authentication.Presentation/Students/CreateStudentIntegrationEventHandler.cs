using MediatR;
using UVS.Common.Application.EventBus;
using UVS.Modules.Authentication.Application.Service;
using UVS.Modules.Authentication.Application.Users.Register;
using UVS.Modules.Authentication.Domain.Users;
using UVS.Modules.System.Integration;

namespace UVS.Modules.Authentication.Presentation.Students;

public sealed class CreateStudentIntegrationEventHandler(ISender sender, IPasswordService passwordService)
    : IntegrationEventHandler<CreateStudentIntegrationEvent>
{
    public override async Task Handle(CreateStudentIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
    {
        
        var password = passwordService.GeneratePassword();
        
        var user = new RegisterUserCommand(integrationEvent.Email, password, integrationEvent.FirstName, integrationEvent.LastName,Role.Student);
        
        var result =await sender.Send(user, cancellationToken);
        
    }
}