using FluentValidation;

namespace UVS.Modules.System.Application.Features.Courses.CreateCourse;

public sealed class CreateCourseCommandValidator:AbstractValidator<CreateCourseCommand>
{
    public CreateCourseCommandValidator()
    {
        RuleFor(x => x.Code).NotEmpty().WithMessage("Code is required");
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(x => x.Description).MaximumLength(500).WithMessage("Description length must be less than 500 characters");
        RuleFor(x => x.CreditHours).GreaterThan(0).LessThan(4);
        RuleFor(x => x.DepartmentId).NotEmpty().WithMessage("DepartmentId is required");
    }
}