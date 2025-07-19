
using UVS.Common.Application.Messaging;

namespace UVS.Modules.System.Application.Features.Instructors.AddInstructorOfficeHours;

public sealed record AddOfficeHourCommand(Guid InstructorId,DayOfWeek Day, TimeSpan StartTime, TimeSpan EndTime):ICommand
{
    
}