using FluentValidation;

namespace UVS.Modules.System.Application.Features.Semesters.CreateSemester;

public sealed class CreateSemesterCommandValidator:AbstractValidator<CreateSemesterCommand>
{
    public CreateSemesterCommandValidator()
    {
        RuleFor(x => x.Name).NotNull().NotEmpty();
        RuleFor(x => x.DepartmentId).NotNull().NotEmpty();
        RuleFor(x => x.AcademicYear).NotNull().NotEmpty(); 
        RuleFor(x=>x.StartDate)
            .NotNull()
            .NotEmpty()
            .Must((cmd,startDate)=>cmd.EndDate >= startDate).WithMessage("End date must be after start date");
    }
    
}