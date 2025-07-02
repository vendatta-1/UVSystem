using UVS.Domain.Common;

namespace UVS.Domain.Schedules;

public  sealed class Schedule:AuditEntity
{
    public Guid CourseId { get; private set; }
    public DayOfWeek Day { get; private set; }
    public TimeSpan StartTime { get; private set; }
    public TimeSpan EndTime { get; private set; }
    public string? Room { get; private set; }

    private Schedule() {}

    public static Schedule Create(Guid courseId, DayOfWeek day, TimeSpan start, TimeSpan end, string? room)
    {
        if (start >= end) throw new ArgumentException("Start time must be before end time");
        return new Schedule
        {
            CourseId = courseId,
            Day = day,
            StartTime = start,
            EndTime = end,
            Room = room
        };
    }
}