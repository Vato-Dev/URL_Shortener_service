using Application.Cqrs.HelperCommands;
using MediatR;
using Quartz;
using Serilog;

namespace Infrastructure.BackgroundJobs;

public class DeleteInvalidUrlsSheduledJob(ISender sender) : IJob
{
    public static readonly JobKey Key = new("delete_expired_short_urls_background_job");

    public async Task Execute(IJobExecutionContext context)
    {
        var (countShorts,countOriginals) = await sender.Send(new DeleteInvalidUrlsCommand());
        Log.Logger.Information($"Deleted expired short urls count : {countShorts} \n Deleted original urls count : {countOriginals}.");
        
    }
}