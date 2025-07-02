using AutoMapper;
using UVS.Application.Messaging;
using UVS.Domain.Common;
using UVS.Domain.Students;

namespace UVS.Application.Students.GetStudents;

public sealed class GetStudentsQueryHandler(IStudentRepository repository, IMapper mapper):
    IQueryHandler<GetStudentsQuery, IReadOnlyList<StudentResponse>>
{
    public async Task<Result<IReadOnlyList<StudentResponse>>> Handle(GetStudentsQuery request, CancellationToken cancellationToken)
    {
        var result = await repository.GetAllAsync();
        
        var response = mapper.Map<IReadOnlyList<StudentResponse>>(result.Value);
        
        return Result.Success(response);
    }
}