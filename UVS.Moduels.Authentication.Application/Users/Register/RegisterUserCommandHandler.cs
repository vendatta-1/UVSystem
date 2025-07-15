 
  
using Microsoft.Extensions.Logging; 
using UVS.Common.Application.Messaging;
using UVS.Common.Domain;
using UVS.Modules.Authentication.Application.Abstractions.Data;
using UVS.Modules.Authentication.Application.Abstractions.Identity;
using UVS.Modules.Authentication.Domain.Users;

namespace UVS.Modules.Authentication.Application.Users.Register;

internal sealed class RegisterUserCommandHandler(
    IIdentityProviderService identityProviderService,
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    ILogger<RegisterUserCommandHandler> logger)
    : ICommandHandler<RegisterUserCommand, Guid>
{
    public async Task<Result<Guid>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        Result<string> result = await identityProviderService.RegisterUserAsync(
            new UserModel(request.Email, request.Password, request.FirstName, request.LastName),
            cancellationToken);

        if (result.IsFailure)
        {
            return Result.Failure<Guid>(result.Error);
        }

        var user = User.Create(request.Email, request.FirstName, request.LastName, result.Value, request.Role);

        await userRepository.CreateAsync(user);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return user.Id;
    }
}