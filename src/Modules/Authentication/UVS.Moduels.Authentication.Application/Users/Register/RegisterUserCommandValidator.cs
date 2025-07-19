using FluentValidation;

namespace UVS.Modules.Authentication.Application.Users.Register;

internal sealed class RegisterUserCommandValidator:AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(cmd => cmd.FirstName).NotEmpty();
        RuleFor(cmd => cmd.LastName).NotEmpty();
        RuleFor(cmd => cmd.Email).NotEmpty().EmailAddress();
        RuleFor(cmd => cmd.Password).NotEmpty().NotNull().MinimumLength(8);
    }
}