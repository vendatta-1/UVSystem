using UVS.Application.Messaging;

namespace UVS.Application.Departments.CreateDepartment;

public record CreateDepartmentCommand(string Name, Guid HeadId, int MaxCreditHoursPerSemester) : ICommand<Guid>
{
  public Guid HeadId { get; init; } = HeadId;
  public string Name { get; init; } = Name;
  public int MaxCreditHoursPerSemester { get; init; } = MaxCreditHoursPerSemester;
   
}