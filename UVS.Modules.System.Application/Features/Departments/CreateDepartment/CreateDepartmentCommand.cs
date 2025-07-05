
using UVS.Common.Application.Messaging;

namespace UVS.Modules.System.Application.Features.Departments.CreateDepartment;

public record CreateDepartmentCommand(string Name, Guid HeadId, int MaxCreditHoursPerSemester) : ICommand<Guid>
{
  public Guid HeadId { get; init; } = HeadId;
  public string Name { get; init; } = Name;
  public int MaxCreditHoursPerSemester { get; init; } = MaxCreditHoursPerSemester;
   
}