using Microsoft.Extensions.Options;
using Quartz;

namespace UVS.Modules.Authentication.Infrastructure.Outbox;

internal class ConfigureOutboxProcessJob(IOptions<OutboxOptions> outbox) :
    IConfigureOptions<QuartzOptions>
{
    private readonly OutboxOptions _outboxOptions = outbox.Value;
    public void Configure(QuartzOptions options)
    { 
        var jobName = typeof(ProcessOutboxJob).FullName;

        options.AddJob<ProcessOutboxJob>(config => config.WithIdentity(jobName!))
            .AddTrigger(config => config
                .ForJob(jobName!)
                    .WithSimpleSchedule(
                        schedule => schedule.WithIntervalInSeconds(_outboxOptions.IntervalInSeconds).RepeatForever()));
    }
}