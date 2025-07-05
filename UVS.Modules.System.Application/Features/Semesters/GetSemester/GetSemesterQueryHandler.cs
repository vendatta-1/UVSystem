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
        var semester = await cacheService.GetAsync<SemesterResponse>($"semester-{request.SemesterId}");
        if (semester!=null)
        {
           return semester;
        }
        var result= await repository.GetAsync(se=>se.Id == request.SemesterId && se.IsDeleted == false);
        if (result.IsSuccess)
        {
            return new SemesterResponse()
            {
                 Name = result.Value.Name,
                 AcademicYear = result.Value.AcademicYear,
                 DepartmentId = result.Value.DepartmentId,
                 StartDate = result.Value.StartDate,
                 EndDate = result.Value.EndDate,
                 IsCurrent = result.Value.IsCurrent,
            };
        }
        return Result.Failure<SemesterResponse>(Error.NotFound("Semester.NotFound",$"Semester with such id:{request.SemesterId} doesn't exist"));
    }
}