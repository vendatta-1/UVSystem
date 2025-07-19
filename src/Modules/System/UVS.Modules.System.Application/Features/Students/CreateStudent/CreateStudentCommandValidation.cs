using FluentValidation;

namespace UVS.Modules.System.Application.Features.Students.CreateStudent;

public sealed class CreateStudentCommandValidation : AbstractValidator<CreateStudentCommand>
{
    public CreateStudentCommandValidation()
    {
        string[] validGenders = ["Female", "Male"];
        RuleFor(x => x.FullName).Length(min: 10, max: 150).WithMessage("the full name must be within valid range");
        RuleFor(x => x.Email).EmailAddress().WithMessage("Email address must follow the email construction rules");
        RuleFor(x => x.DateOfBirth).GreaterThan(DateTime.UtcNow.AddYears(-40));
        RuleFor(x => x.Gender).Must(x => validGenders.Contains(x)).WithMessage($"gender must be {validGenders[0] } or {validGenders[1]}");
        RuleFor(x => x.NationalId).Must(nat => nat.Length == 14);
        RuleFor(x => x.Phone).Must(pho => pho.Length == 11);
    }
}
