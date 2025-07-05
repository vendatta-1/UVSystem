using AutoMapper;
using UVS.Common.Application.Messaging;
using UVS.Common.Domain;
using UVS.Domain.Common;
using UVS.Domain.Students;

namespace UVS.Modules.System.Application.Features.Students.GetStudent;

public class GetStudentQueryHandler(IStudentRepository repository,IMapper mapper) :
    IQueryHandler<GetStudentQuery,StudentResponse>
{
    public async Task<Result<StudentResponse>> Handle(GetStudentQuery request, CancellationToken cancellationToken)
    {
        var result =await repository.GetByIdAsync(request.StudentId);
        
        var studentResponse = mapper.Map<StudentResponse>(result);
        
        return studentResponse;
    }
}