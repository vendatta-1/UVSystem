using FluentValidation;

namespace UVS.Modules.Authentication.Application.Users.Login;

internal sealed class LoginCommandValidator:AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Username).NotEmpty().EmailAddress().WithMessage("Invalid username");
        RuleFor(x => x.Password).NotEmpty().Length(min:8, max:100);
    }
}