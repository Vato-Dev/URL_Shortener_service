using Application.Abstractions;
using MediatR;

namespace Application.Cqrs.ShortUrls.Commands;

public sealed class DeleteExpiredShortUrlsCommand 
    : IRequest<int>;



internal class DeleteExpiredShortUrlsCommandHandler(IShortUrlRepository repository) 
    : IRequestHandler<DeleteExpiredShortUrlsCommand, int>
{
    public async Task<int> Handle(DeleteExpiredShortUrlsCommand request, CancellationToken cancellationToken)
    {
        var expiredFrom = DateTime.UtcNow.AddDays(-30);
        var deletedUrlsCoint = await repository.DeleteExpiredUrlsAsync(expiredFrom,cancellationToken);
        return deletedUrlsCoint;
    }
}