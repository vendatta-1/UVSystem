using UVS.Application.Messaging;

namespace UVS.Application.Departments.GetDepartment;

public record   GetDepartmentQuery(Guid DepartmentId):IQuery<DepartmentResponse>
{
    public Guid DepartmentId { get; init; } = DepartmentId;
}