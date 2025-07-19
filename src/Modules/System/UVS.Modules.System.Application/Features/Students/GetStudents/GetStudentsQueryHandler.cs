using AutoMapper;
using UVS.Common.Application.Caching;
using UVS.Common.Application.Messaging;
using UVS.Common.Domain;
using UVS.Domain.Common;
using UVS.Domain.Students;

namespace UVS.Modules.System.Application.Features.Students.GetStudents;

public sealed class GetStudentsQueryHandler(IStudentRepository repository, IMapper mapper, ICacheService cacheService):
    IQueryHandler<GetStudentsQuery, IReadOnlyList<StudentResponse>>
{
    public async Task<Result<IReadOnlyList<StudentResponse>>> Handle(GetStudentsQuery request, CancellationToken cancellationToken)
    {
        var key = $"students";

        var students = await cacheService.GetAsync<IReadOnlyList<StudentResponse>>(key, cancellationToken);

        if (students is not null && students.Count > 0)
        {
            return  Result.Success(students);
        }
        var result = await repository.GetAllAsync();
        
        var response = mapper.Map<IReadOnlyList<StudentResponse>>(result);
        if (result.Count > 0)
        {
            await cacheService.SetAsync(key, response, cancellationToken: cancellationToken);
        }
        
        return Result.Success(response);
    }
}