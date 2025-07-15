using UVS.Common.Application.Messaging;
using UVS.Common.Domain;
using UVS.Domain.Courses;
using UVS.Domain.Departments;
using UVS.Domain.Instructors;
using UVS.Domain.Semesters;
using UVS.Modules.System.Application.Data;

namespace UVS.Modules.System.Application.Features.Courses.CreateCourse;

internal sealed class CreateCourseCommandHandler(ISemesterRepository semesterRepository,
    ICourseRepository courseRepository,
    IInstructorRepository instructorRepository,
    IDepartmentRepository departmentRepository,
    IUnitOfWork unitOfWork) :
    ICommandHandler<CreateCourseCommand,Guid>
{
    public async Task<Result<Guid>> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
    {    
        var deptExists = await departmentRepository.ExistsAsync(x=>x.Id == request.DepartmentId);
        if (!deptExists)
        {
            return Result.Failure<Guid>(Error.NotFound("Department.NotFound",$"There is non department has this id {request.DepartmentId}"));
        }
        var semesterResult = await semesterRepository.GetByIdAsync(request.SemesterId);
        if (semesterResult==null)
        {
            return Result.Failure<Guid>(Error.NotFound("Semester.NotFound",$"There is non semester has this id {request.SemesterId}"));
        }

        if (request.InstructorId.HasValue && request.InstructorId.Value != Guid.Empty)
        {
            var instructorExists = await instructorRepository.ExistsAsync(x => x.Id == request.InstructorId);
            if (!instructorExists)
            {
                return Result.Failure<Guid>(Error.NotFound("Instructor.NotFound",$"the instructor does not exist with {request.InstructorId}"));
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
        var semester = semesterResult;
        
        semester.AddSemesterCourse(course);
        
        await courseRepository.CreateAsync(course);
        
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return course.Id;
        
    }
}