using AutoMapper;
using UVS.Common.Application.Messaging;
using UVS.Common.Domain;
using UVS.Domain.Common;
using UVS.Domain.Departments;
using UVS.Domain.Students;
using UVS.Modules.System.Application.Data;

namespace UVS.Modules.System.Application.Features.Students.CreateStudent;

public class CreateStudentCommandHandler(IStudentRepository repository,IUnitOfWork unitOfWork,IDepartmentRepository departmentRepository,IMapper mapper):
    ICommandHandler<CreateStudentCommand,Guid>
{
    public async Task<Result<Guid>> Handle(CreateStudentCommand request, CancellationToken cancellationToken)
    {
         var result = await departmentRepository.GetAsync(dept=> dept.Name.ToLower() == request.DepartmentName.ToLower());
         
         if (result == null)
         {
             return Result.Failure<Guid>(Error.NotFound("Department.NotFound", $"The department {request.DepartmentName} was not found"));
         }
         


         var student = Student.Create(
             request.FullName,
             request.NationalId,
             request.Email,
             request.Phone,
             request.DateOfBirth,
             request.Gender,
             result.Id,
             request.Level,
             new Address(request.City,request.City,request.Town)
         );
         
         var createResult = await repository.CreateAsync(student);
         
         await unitOfWork.SaveChangesAsync(cancellationToken);

         return student.Id;
    }
}