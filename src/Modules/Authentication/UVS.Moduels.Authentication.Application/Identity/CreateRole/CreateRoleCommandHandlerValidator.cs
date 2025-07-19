using FluentValidation;

namespace UVS.Modules.Authentication.Application.Identity.CreateRole;

internal  sealed class CreateRoleCommandHandlerValidator:AbstractValidator<CreateRoleCommand>
{
    public CreateRoleCommandHandlerValidator()
    {
        RuleFor(x => x.Name).NotEmpty().NotNull();
        RuleFor(x => x.Description).NotEmpty().NotNull();
    }
}