using UVS.Common.Domain;

namespace UVS.Domain.Departments;

public sealed class CreateDepartmentDomainEvent(Guid departmentId): DomainEvent
{
    public Guid DepartmentId { get; init; } = departmentId;
}