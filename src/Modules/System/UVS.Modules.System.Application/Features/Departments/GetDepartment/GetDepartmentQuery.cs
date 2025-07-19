 
using UVS.Common.Application.Messaging;

namespace UVS.Modules.System.Application.Features.Departments.GetDepartment;

public sealed record GetDepartmentQuery(Guid DepartmentId):IQuery<DepartmentResponse>
{
    public Guid DepartmentId { get; init; } = DepartmentId;
}