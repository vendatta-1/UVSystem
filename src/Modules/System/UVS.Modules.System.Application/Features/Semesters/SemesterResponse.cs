using UVS.Domain.Students;

namespace UVS.Modules.System.Application.Features.Semesters;

public sealed record SemesterResponse
{
    public string Name { get; init; } = null!;
    public DateTime StartDate { get;   init; }
    public DateTime EndDate { get;   init; }
    public bool IsCurrent { get;   init; }
    public Guid DepartmentId { get;   init; }
    public AcademicLevel AcademicYear { get;   init; }
}