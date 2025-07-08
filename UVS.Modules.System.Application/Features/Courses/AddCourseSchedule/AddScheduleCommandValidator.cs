using FluentValidation;

namespace UVS.Modules.System.Application.Features.Courses.AddCourseSchedule;

public sealed class AddScheduleCommandValidator : AbstractValidator<AddScheduleCommand>
{
    public AddScheduleCommandValidator()
    {
        RuleFor(x => x.StartTime).
            Must((cmd, start)=>(cmd.EndTime-start).TotalHours>=2).
            WithMessage("Total hours must be greater than 2 hours.");
            
    }
}