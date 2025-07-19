using UVS.Common.Application.Messaging;
using UVS.Common.Domain;
using UVS.Domain.Departments;
using UVS.Domain.Instructors;
using UVS.Modules.System.Application.Data;

namespace UVS.Modules.System.Application.Features.Departments.SetDepartmentHead;

internal sealed class SetDepartmentHeadCommandHandler
    (IInstructorRepository instructorRepository, 
        IDepartmentRepository departmentRepository,
        IUnitOfWork unitOfWork) : ICommandHandler<SetDepartmentHeadCommand>
{
    public async Task<Result> Handle(SetDepartmentHeadCommand request, CancellationToken cancellationToken)
    {
         var  instructor = await instructorRepository.GetByIdAsync(request.HeadId);
         if (instructor == null)
         {
             return Result.Failure(Error.NullValue);
         }
         var department = await departmentRepository.GetByIdAsync(request.DepartmentId);

         if (department == null)
         {
             return Result.Failure(Error.NullValue);
         }
         
         department.ChangeHeadId(request.HeadId);
         
         await unitOfWork.SaveChangesAsync(cancellationToken);
         
         return Result.Success();
         
    } 
}