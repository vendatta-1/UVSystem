using FluentValidation;

namespace UVS.Modules.Authentication.Application.Identity.GetRoleId;

internal sealed class GetRoleIdCommandValidator:AbstractValidator<GetRoleIdCommand>
{
    public GetRoleIdCommandValidator()
    {
        RuleFor(r => r.RoleName).NotNull().NotEmpty().NotNull();
    }
}