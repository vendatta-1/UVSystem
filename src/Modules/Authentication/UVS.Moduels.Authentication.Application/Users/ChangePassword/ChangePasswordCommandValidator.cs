using FluentValidation;

namespace UVS.Modules.Authentication.Application.Users.ChangePassword;

internal sealed class ChangePasswordCommandValidator:AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordCommandValidator()
    {
        RuleFor(command => command.Username).NotEmpty().NotNull();
        RuleFor(command => command.OldPassword).NotEmpty().NotNull();
        RuleFor(command => command.NewPassword).NotEmpty().NotNull();
    }
}