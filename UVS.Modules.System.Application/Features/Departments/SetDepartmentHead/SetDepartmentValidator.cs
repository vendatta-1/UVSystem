using FluentValidation;

namespace UVS.Modules.System.Application.Features.Departments.SetDepartmentHead;

internal sealed class SetDepartmentValidator:AbstractValidator<SetDepartmentHeadCommand>
{
    public SetDepartmentValidator()
    {
        RuleFor(x => x.HeadId).NotNull().NotEmpty();
    }
}