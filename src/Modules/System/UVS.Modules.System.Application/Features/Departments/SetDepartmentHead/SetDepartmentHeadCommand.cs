using UVS.Common.Application.Messaging;

namespace UVS.Modules.System.Application.Features.Departments.SetDepartmentHead;

public sealed class SetDepartmentHeadCommand(Guid headId, Guid departmentId):ICommand
{
    public Guid HeadId { get; init; } = headId;
    public Guid DepartmentId { get; init; } = departmentId;
}