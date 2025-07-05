using UVS.Common.Application.Messaging;
using UVS.Common.Domain;
using UVS.Domain.Common;
using UVS.Domain.Departments;
using UVS.Domain.Instructors;
using UVS.Modules.System.Application.Data;

namespace UVS.Modules.System.Application.Features.Departments.CreateDepartment;

public sealed class CreateDepartmentCommandHandler(IDepartmentRepository departmentRepository,
    IInstructorRepository instructorRepository,IUnitOfWork unitOfWork):ICommandHandler<CreateDepartmentCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateDepartmentCommand request, CancellationToken cancellationToken)
    {
        var userExists = await instructorRepository.GetByIdAsync(request.HeadId);
        if (userExists.IsFailure || userExists.Value == null)
        {
            return Result.Failure<Guid>(userExists.Error);
        }
        
        var department = Department.Create(request.Name, request.HeadId, request.MaxCreditHoursPerSemester);
        var createdDepartment = await departmentRepository.CreateAsync(department);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success(createdDepartment.Value);
    }
}