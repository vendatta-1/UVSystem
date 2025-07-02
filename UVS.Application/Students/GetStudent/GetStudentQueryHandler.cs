using AutoMapper;
using UVS.Application.Messaging;
using UVS.Domain.Common;
using UVS.Domain.Students;

namespace UVS.Application.Students.GetStudent;

public class GetStudentQueryHandler(IStudentRepository repository,IMapper mapper):IQueryHandler<GetStudentQuery,StudentResponse>
{
    public async Task<Result<StudentResponse>> Handle(GetStudentQuery request, CancellationToken cancellationToken)
    {
        var result =await repository.GetByIdAsync(request.StudentId);
        
        var studentResponse = mapper.Map<StudentResponse>(result);
        
        return studentResponse;
    }
}