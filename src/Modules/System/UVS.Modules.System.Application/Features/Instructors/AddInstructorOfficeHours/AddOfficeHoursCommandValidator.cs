using FluentValidation;

namespace UVS.Modules.System.Application.Features.Instructors.AddInstructorOfficeHours;

public sealed class AddOfficeHoursCommandValidator:AbstractValidator<AddOfficeHourCommand>
{
    public AddOfficeHoursCommandValidator()
    {
        RuleFor(ins => ins.StartTime)
            .Must((ins, start) => ins.EndTime > ins.StartTime)
            .Must((ins, time) => (ins.EndTime - ins.StartTime).TotalHours <= 6)
            .Must((ins, time) => (ins.EndTime - ins.StartTime).TotalHours >= 2);
    }
}