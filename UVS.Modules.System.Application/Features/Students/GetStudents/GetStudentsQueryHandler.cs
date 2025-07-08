using AutoMapper; 
using UVS.Common.Application.Messaging;
using UVS.Common.Domain;
using UVS.Domain.Common;
using UVS.Domain.Students;

namespace UVS.Modules.System.Application.Features.Students.GetStudents;

public sealed class GetStudentsQueryHandler(IStudentRepository repository, IMapper mapper):
    IQueryHandler<GetStudentsQuery, IReadOnlyList<StudentResponse>>
{
    public async Task<Result<IReadOnlyList<StudentResponse>>> Handle(GetStudentsQuery request, CancellationToken cancellationToken)
    {
        var result = await repository.GetAllAsync();
        
        var response = mapper.Map<IReadOnlyList<StudentResponse>>(result);
        
        return Result.Success(response);
    }
}