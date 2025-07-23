using Microsoft.Extensions.Logging;
using UVS.Common.Application.Clock;
using UVS.Domain.Schedules;
using UVS.Modules.System.Infrastructure.Data;

namespace UVS.Modules.System.Infrastructure.Repositories;

internal sealed class ScheduleRepository(UVSDbContext context, ILogger<ScheduleRepository> logger, IDateTimeProvider dateTimeProvider) :
    Repository<Schedule>(context, logger, dateTimeProvider) ,IScheduleRepository
{
    
}