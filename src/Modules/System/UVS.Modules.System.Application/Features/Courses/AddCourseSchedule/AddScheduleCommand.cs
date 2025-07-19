using UVS.Common.Application.Messaging;

namespace UVS.Modules.System.Application.Features.Courses.AddCourseSchedule;

public sealed record AddScheduleCommand:ICommand
{
    public Guid CourseId { get;   init; }
    public DayOfWeek Day { get;   init; }
    public TimeSpan StartTime { get;   init; }
    public TimeSpan EndTime { get;   init; }
    public string? Room { get;   init; }

}