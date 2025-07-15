using UVS.Common.Application.Messaging;
using UVS.Common.Domain;
using UVS.Domain.Common;
using UVS.Domain.Departments;
using UVS.Domain.Instructors;
using UVS.Modules.System.Application.Data;

namespace UVS.Modules.System.Application.Features.Departments.CreateDepartment;

internal sealed class CreateDepartmentCommandHandler(IDepartmentRepository departmentRepository,
    IInstructorRepository instructorRepository,IUnitOfWork unitOfWork):ICommandHandler<CreateDepartmentCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateDepartmentCommand request, CancellationToken cancellationToken)
    {
        Instructor? head = null;
        if (request.HeadId != null)
        {
            head = await instructorRepository.GetByIdAsync(request.HeadId.Value);
        }

        Department? department = null;

        department = Department.Create(request.Name, head is null ? null : request.HeadId, request.MaxCreditHoursPerSemester);

        var deptId = await departmentRepository.CreateAsync(department);
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return deptId;
    }
}