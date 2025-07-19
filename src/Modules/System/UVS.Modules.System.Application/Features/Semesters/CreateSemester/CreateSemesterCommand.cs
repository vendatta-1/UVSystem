using UVS.Common.Application.Messaging;
using UVS.Domain.Students;

namespace UVS.Modules.System.Application.Features.Semesters.CreateSemester;

public sealed record CreateSemesterCommand : ICommand<Guid>
{
    public string Name { get; init; } = null!;
    public DateTime StartDate { get;   init; }
    public DateTime EndDate { get;   init; }
    public bool IsCurrent { get;   init; }
    public Guid DepartmentId { get;   init; }
    public AcademicLevel AcademicYear { get;   init; }
    
}