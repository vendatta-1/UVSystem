using UVS.Common.Application.Messaging;
using UVS.Common.Domain;
using UVS.Domain.Courses;
using UVS.Domain.Departments;
using UVS.Domain.Instructors;
using UVS.Domain.Semesters;
using UVS.Modules.System.Application.Data;

namespace UVS.Modules.System.Application.Features.Courses.CreateCourse;

public sealed class CreateCourseCommandHandler(ISemesterRepository semesterRepository,
    ICourseRepository courseRepository,
    IInstructorRepository instructorRepository,
    IDepartmentRepository departmentRepository,
    IUnitOfWork unitOfWork) :
    ICommandHandler<CreateCourseCommand,Guid>
{
    public async Task<Result<Guid>> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
    {    
        var deptExists = await departmentRepository.ExistsAsync(x=>x.Id == request.DepartmentId);
        if (deptExists.IsFailure)
        {
            return Result.Failure<Guid>(deptExists.Error);
        }
        var semesterResult = await semesterRepository.GetByIdAsync(request.SemesterId);
        if (semesterResult.IsFailure)
        {
            return Result.Failure<Guid>(semesterResult.Error);
        }

        if (request.InstructorId.HasValue && request.InstructorId.Value != Guid.Empty)
        {
            var instructorExists = await instructorRepository.ExistsAsync(x => x.Id == request.InstructorId);
            if (instructorExists.IsFailure)
            {
                return Result.Failure<Guid>(instructorExists.Error);
            }
        }

        var course = Course.Create
        (
            request.Code,
            request.Name,
            request.CreditHours,
            request.DepartmentId,
            request.InstructorId,
            request.Description
        );
        var semester = semesterResult.Value;
        
        semester.AddSemesterCourse(course);
        
        var createResult = await courseRepository.CreateAsync(course);

        if (createResult.IsFailure)
        {
            return Result.Failure<Guid>(createResult.Error);
        }
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return course.Id;
        
    }
}