using FluentValidation;

namespace UVS.Application.Students.CreateStudent;

public sealed class CreateStudentCommandValidation : AbstractValidator<CreateStudentCommand>
{
    public CreateStudentCommandValidation()
    {
        string[] validGenders = ["Female", "Male"];
        RuleFor(x => x.FullName).Length(min: 20, max: 150);
        RuleFor(x => x.Email).EmailAddress();
        RuleFor(x => x.DateOfBirth).GreaterThan(DateTime.UtcNow.AddYears(-40));
        RuleFor(x => x.Gender).Must(x => validGenders.Contains(x));
        RuleFor(x => x.NationalId).Must(nat => nat.Length == 14);
        RuleFor(x => x.Phone).Must(pho => pho.Length == 11);
    }
}
