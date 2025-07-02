using UVS.Domain.Common;

namespace UVS.Domain.Departments;

public sealed class ChangeDepartmentHeadDomainEvent(Guid departmentId) : DomainEvent
{
  public Guid DepartmentId { get; init; } = departmentId;
}