using UVS.Common.Application.Caching;
using UVS.Common.Application.Messaging;
using UVS.Common.Domain;
using UVS.Domain.Courses;
using UVS.Domain.Departments;
using UVS.Domain.Students;

namespace UVS.Modules.System.Application.Features.Departments.GetDepartment;

internal sealed class GetDepartmentQueryHandler(IDepartmentRepository  departmentRepository,
    ICourseRepository courseRepository,
    ICacheService cacheService,
    IStudentRepository studentRepository)
    :IQueryHandler<GetDepartmentQuery, DepartmentResponse>
{
    public async Task<Result<DepartmentResponse>> Handle(GetDepartmentQuery request, CancellationToken cancellationToken)
    {
        var cachedDepartment = await cacheService.GetAsync<DepartmentResponse>($"department-{request.DepartmentId}",cancellationToken);
        if (cachedDepartment is not null)
        {
            return cachedDepartment;
        }
        
        var department = await departmentRepository.GetByIdAsync(request.DepartmentId);

        if (department is null)
        {
            return Result.Failure<DepartmentResponse>(Error.NotFound("Department.NotFound", $"the department {request.DepartmentId} was not found."));
        }
        
        var studentsCount = await studentRepository.CountAsync(x=>x.DepartmentId == request.DepartmentId);
        
        var coursesCount = await courseRepository.CountAsync(x => x.DepartmentId == request.DepartmentId);


        var response = new DepartmentResponse()
        {
            StudentsCount = studentsCount,
            CoursesCount = coursesCount,
            MaxCreditHoursPerSemester = department.MaxCreditHoursPerSemester,
            Id = department.Id,
            Name = department.Name
        };
        await cacheService.SetAsync($"department-{request.DepartmentId}",response,cancellationToken: cancellationToken);

        return response;
    }
}