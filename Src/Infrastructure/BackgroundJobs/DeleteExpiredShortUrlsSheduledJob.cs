using Application.Cqrs.ShortUrls.Commands;
using MediatR;
using Quartz;
using Quartz.Logging;
using Serilog;

namespace Infrastructure.BackgroundJobs;

public class DeleteExpiredShortUrlsSheduledJob(ISender sender,CancellationToken ct) : IJob
{
    public static readonly JobKey Key = new("delete_expired_short_urls_background_job");

    public async Task Execute(IJobExecutionContext context)
    {
        var count = await sender.Send(new DeleteExpiredShortUrlsCommand(),ct);
        Log.Logger.Information($"Deleted expired short urls count : {count}.");
        
    }
}