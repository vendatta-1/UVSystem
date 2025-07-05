using UVS.Common.Application.Messaging;

namespace UVS.Modules.System.Application.Features.Semesters.GetSemester;

public sealed record GetSemesterQuery(Guid SemesterId):IQuery<SemesterResponse>
{
    
}