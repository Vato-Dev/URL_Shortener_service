using Application.Abstractions;
using MediatR;

namespace Application.Cqrs.HelperCommands;

public sealed class DeleteInvalidUrlsCommand 
    : IRequest<(int deletedShortUrlsCount , int deletedRegularUrlsCount)>;



internal class DeleteInvalidUrlsCommandHandler(IShortUrlRepository repository, IRegularUrlRepository regularUrlRepository)  // i know it's not good practice and violations of principles to inject other models repo
    : IRequestHandler<DeleteInvalidUrlsCommand, (int deletedShortUrlsCount , int deletedRegularUrlsCount)>
{
    public async Task<(int deletedShortUrlsCount , int deletedRegularUrlsCount)> Handle(DeleteInvalidUrlsCommand request, CancellationToken cancellationToken)
    {
        var expiredFrom = DateTime.UtcNow.AddDays(-30);
        
        var (countOfDeletedShortUrls,regularUrlId) = await repository.DeleteExpiredUrlsAsync(expiredFrom,cancellationToken);
        var countOfDeletedRegularUrls =  await regularUrlRepository.DeleteAllOrphanUrls(regularUrlId,cancellationToken);
        return (countOfDeletedShortUrls , countOfDeletedRegularUrls);
    }
}