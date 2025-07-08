using UVS.Common.Application.Caching;
using UVS.Common.Application.Messaging;
using UVS.Common.Domain;
using UVS.Domain.Semesters;

namespace UVS.Modules.System.Application.Features.Semesters.GetSemester;

public sealed class GetSemesterQueryHandler(ISemesterRepository  repository,ICacheService cacheService)
    :IQueryHandler<GetSemesterQuery,SemesterResponse>
{
    public async Task<Result<SemesterResponse>> Handle(GetSemesterQuery request, CancellationToken cancellationToken)
    {
        var semesterCache = await cacheService.GetAsync<SemesterResponse>($"semester-{request.SemesterId}", cancellationToken);
        if (semesterCache!=null)
        {
           return semesterCache;
        }
        var semester = await repository.GetAsync(se=>se.Id == request.SemesterId && se.IsDeleted == false);
        if (semester!=null)
        {
            return new SemesterResponse()
            {
                 Name = semester.Name,
                 AcademicYear = semester.AcademicYear,
                 DepartmentId = semester.DepartmentId,
                 StartDate = semester.StartDate,
                 EndDate = semester.EndDate,
                 IsCurrent = semester.IsCurrent,
            };
        }
        return Result.Failure<SemesterResponse>(Error.NotFound("Semester.NotFound",$"Semester with such id:{request.SemesterId} doesn't exist"));
    }
}