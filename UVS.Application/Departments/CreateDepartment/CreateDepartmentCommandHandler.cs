using UVS.Application.Caching;
using UVS.Application.Data;
using UVS.Application.Messaging;
using UVS.Domain.Common;
using UVS.Domain.Departments;
using UVS.Domain.Instructors;

namespace UVS.Application.Departments.CreateDepartment;

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