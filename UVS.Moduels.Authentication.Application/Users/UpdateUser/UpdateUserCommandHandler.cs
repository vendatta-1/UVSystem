using UVS.Authentication.Domain.Users;
using UVS.Common.Application.Messaging;
using UVS.Common.Domain;
using UVS.Modules.Authentication.Application.Abstractions.Data;

namespace UVS.Modules.Authentication.Application.Users.UpdateUser;

internal sealed class UpdateUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
    : ICommandHandler<UpdateUserCommand>
{
    public async Task<Result> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        User? user = await userRepository.GetByIdAsync(request.UserId);

        if (user is null)
        {
            return Result.Failure(UserErrors.NotFound(request.UserId));
        }

        user.Update(request.FirstName, request.LastName);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
