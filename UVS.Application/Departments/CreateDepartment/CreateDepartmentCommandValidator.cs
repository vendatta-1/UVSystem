using FluentValidation;

namespace UVS.Application.Departments.CreateDepartment;

public sealed class CreateDepartmentCommandValidator:AbstractValidator<CreateDepartmentCommand>
{
    public CreateDepartmentCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x=>x.MaxCreditHoursPerSemester).GreaterThan(0).LessThan(24);
    }
}