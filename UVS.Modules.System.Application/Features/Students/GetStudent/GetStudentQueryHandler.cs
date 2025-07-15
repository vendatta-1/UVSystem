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
        if (result == null)
        {
            return Result.Failure<StudentResponse>(Error.NotFound("Student.NotFound", $"the student {request.StudentId} was not found"));
        }
        var studentResponse = new StudentResponse()
        {
            FirstName = result.FullName.Split(' ').First(),
            LastName = result.FullName.Split(' ').Last(),
            Email = result.Email,
            DepartmentId = result.DepartmentId,
            DateOfBirth = result.DateOfBirth,
            Gender = result.Gender,
            NationalId = result.NationalId,
            Phone = result.Phone
        };
        
        return studentResponse;
    }
}