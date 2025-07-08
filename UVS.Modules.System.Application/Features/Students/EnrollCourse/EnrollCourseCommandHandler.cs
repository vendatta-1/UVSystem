using UVS.Common.Application.Clock;
using UVS.Common.Application.Messaging;
using UVS.Common.Domain;
using UVS.Domain.Common;
using UVS.Domain.Courses;
using UVS.Domain.Students;
using UVS.Modules.System.Application.Data;

namespace UVS.Modules.System.Application.Features.Students.EnrollCourse;

public class EnrollCourseCommandHandler(
    IDateTimeProvider dateTime,
    IUnitOfWork unitOfWork,
    IStudentRepository studentRepository,
    ICourseRepository courseRepository)
    : ICommandHandler<EnrollCourseCommand>
{
    public async Task<Result> Handle(EnrollCourseCommand request, CancellationToken cancellationToken)
    {
        var student = await studentRepository.GetByIdAsync(request.StudentId);
        if (student==null )
            return Result.Failure(Error.NotFound("Student.NotFound","Student not found"));

        var course = await courseRepository.GetByIdAsync(request.CourseId);
        if (course==null )
            return Result.Failure(Error.NotFound("Course.NotFound","Course not found"));

        try
        {
            student.EnrollInCourse(course, dateTime.UtcNow);
        }
        catch (Exception ex)
        {
            return Result.Failure(Error.Failure("System.Error", ex.Message));
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}
