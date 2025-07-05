
using Microsoft.EntityFrameworkCore;

namespace UVS.Domain.Instructors;

[Owned]
public record OfficeHour
{
    public OfficeHour() { }
    public DayOfWeek Day { get; private set; }
    public TimeSpan StartTime { get; private set; }
    public TimeSpan EndTime { get; private set; }

    public OfficeHour(DayOfWeek day, TimeSpan startTime, TimeSpan endTime)
    {
        if (startTime >= endTime)
            throw new ArgumentException("StartTime must be earlier than EndTime.");

        if ((endTime - startTime).TotalHours > 12)
            throw new ArgumentException("Office hour duration cannot exceed 12 hours.");

        Day = day;
        StartTime = startTime;
        EndTime = endTime;
    }
}
