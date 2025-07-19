using Microsoft.Extensions.Options;
using Quartz;

namespace UVS.Modules.System.Infrastructure.Inbox;

internal class ConfigureInboxProcessJob(IOptions<InboxOptions> options) :
    IConfigureOptions<QuartzOptions>
{
    private readonly InboxOptions _inboxOptions = options.Value;


    public void Configure(QuartzOptions options)
    {
        var jobName = nameof(ConfigureInboxProcessJob);
        options.AddJob<ProcessInboxJob>(configure =>
        {
            configure.WithIdentity(jobName);
        })
        .AddTrigger(trigger =>
        {
            trigger.ForJob(jobName)
                .WithSimpleSchedule(schedule =>
                    schedule.WithIntervalInSeconds(_inboxOptions.IntervalInSeconds).RepeatForever());
        });

    }
}