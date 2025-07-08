 
using UVS.Common.Domain;

namespace UVS.Domain.Departments;

public sealed class ChangeDepartmentHeadDomainEvent(Guid departmentId, Guid headId) : DomainEvent
{
  public Guid DepartmentId { get; init; } = departmentId;
  public Guid HeadId { get; init; } = headId;
}