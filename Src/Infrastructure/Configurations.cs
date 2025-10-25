using Application;
using Infrastructure.BackgroundJobs;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace Infrastructure;

public class Configurations : ConfigurationBase 
{
    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddQuartz(q =>
        {
            q.AddJob<DeleteInvalidUrlsSheduledJob>(DeleteInvalidUrlsSheduledJob.Key, job => job.StoreDurably())
                .AddTrigger(trigger => trigger
                    .ForJob(DeleteInvalidUrlsSheduledJob.Key).StartAt(DateTimeOffset.Now.AddSeconds(10))
                    .WithSimpleSchedule(schedule => schedule
                        .WithInterval(TimeSpan.FromHours(48))
                        .RepeatForever()));
        });
        services.AddQuartzHostedService();
    }
}