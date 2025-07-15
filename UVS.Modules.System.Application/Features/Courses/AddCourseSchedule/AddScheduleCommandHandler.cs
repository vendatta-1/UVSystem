using UVS.Common.Application.Messaging;
using UVS.Common.Domain;
using UVS.Domain.Courses;
using UVS.Domain.Schedules;
using UVS.Modules.System.Application.Data;

namespace UVS.Modules.System.Application.Features.Courses.AddCourseSchedule;

internal sealed class AddScheduleCommandHandler(IScheduleRepository scheduleRepository, IUnitOfWork unitOfWork,ICourseRepository courseRepository)
    : ICommandHandler<AddScheduleCommand>
{
    public async Task<Result> Handle(AddScheduleCommand request, CancellationToken cancellationToken)
    { 
        var course = await courseRepository.GetByIdAsync(request.CourseId);
        if (course == null)
        {
            return Result.Failure(Error.NotFound("Course.NotFound", $"The course {request.CourseId} was not found."));
        }

        if (!(await IsValid(request)))
        {
            return Result.Failure(Error.Conflict("Course.Conflict",$"there is a conflict of this schedule with exists."));
        }

        var schedule = Schedule.Create(request.CourseId, request.Day, request.StartTime, request.EndTime, request.Room);
        course.AddSchedule(schedule);
        await courseRepository.UpdateAsync(course);
        await scheduleRepository.CreateAsync(schedule);
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }

    private async Task<bool> IsValid(AddScheduleCommand request)
    {
        var course = await courseRepository.GetByIdAsync(request.CourseId);

        if (!course.IsAvailable)
            return false;

        if (course.Schedules.Count >= 3)
            return false;

        var overlaps = course.Schedules.Any(existing =>
            existing.Day == request.Day &&
            existing.Room == request.Room &&
            existing.StartTime < request.EndTime &&
            request.StartTime < existing.EndTime
        );

        return !overlaps;
    }

}