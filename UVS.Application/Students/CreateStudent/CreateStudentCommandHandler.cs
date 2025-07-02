using AutoMapper;
using UVS.Application.Data;
using UVS.Application.Messaging;
using UVS.Domain.Common;
using UVS.Domain.Departments;
using UVS.Domain.Students;

namespace UVS.Application.Students.CreateStudent;

public class CreateStudentCommandHandler(IStudentRepository repository,IUnitOfWork unitOfWork,IDepartmentRepository departmentRepository,IMapper mapper):ICommandHandler<CreateStudentCommand,Guid>
{
    public async Task<Result<Guid>> Handle(CreateStudentCommand request, CancellationToken cancellationToken)
    {
         var result =await departmentRepository.GetAsync(dept=>dept.Name == request.DepartmentName);
         if (result.IsFailure)
         {
             return Result.Failure<Guid>(Error.NotFound("Department.NotFound", request.DepartmentName));
         }
         


         var student = Student.Create(
             request.FullName,
             request.NationalId,
             request.Email,
             request.Phone,
             request.DateOfBirth,
             request.Gender,
             result.Value.Id
         );
         
         var createResult = await repository.CreateAsync(student);
         
         
         if (createResult.IsFailure)
         {
             return Result.Failure<Guid>(Error.Failure("Student.CreateFailed", result.Error.Description));
         }

         await unitOfWork.SaveChangesAsync(cancellationToken);

         return createResult;
    }
}