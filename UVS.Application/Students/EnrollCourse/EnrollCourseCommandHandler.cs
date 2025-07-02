using UVS.Application.Clock;
using UVS.Application.Data;
using UVS.Application.Messaging;
using UVS.Domain.Common;
using UVS.Domain.Courses;
using UVS.Domain.Enrollments;
using UVS.Domain.Students;

namespace UVS.Application.Students.EnrollCourse;

public class EnrollCourseCommandHandler(
    IDateTimeProvider dateTime,
    IUnitOfWork unitOfWork,
    IStudentRepository studentRepository,
    ICourseRepository courseRepository)
    : ICommandHandler<EnrollCourseCommand>
{
    public async Task<Result> Handle(EnrollCourseCommand request, CancellationToken cancellationToken)
    {
        var studentResult = await studentRepository.GetByIdAsync(request.StudentId);
        if (studentResult.IsFailure || studentResult.Value == null)
            return Result.Failure(Error.NotFound("Student.NotFound","Student not found"));

        var courseResult = await courseRepository.GetByIdAsync(request.CourseId);
        if (courseResult.IsFailure || courseResult.Value == null)
            return Result.Failure(Error.NotFound( "Course.NotFound","Course not found"));

        var student = studentResult.Value;
        var course = courseResult.Value;

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
