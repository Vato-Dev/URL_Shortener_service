using Application.Abstractions;
using Domain.Models;
using MediatR;

namespace Application.Cqrs.ShortUrls.Queries;

public sealed record GetShortUrlQuery(Guid Id) : IRequest<GetShortUrlResult>;

public sealed record GetShortUrlResult(bool IsSuccess, ShortUrl? Data = null, string? Error = null);

internal sealed class GetShortUrlQueryHandler(IShortUrlRepository repository)
    : IRequestHandler<GetShortUrlQuery, GetShortUrlResult>
{
    public async Task<GetShortUrlResult> Handle(GetShortUrlQuery request, CancellationToken ct)
    {
        if (request.Id == Guid.Empty)
            return new(false, Error: "Invalid Id");

        var entity = await repository.GetByIdAsync(request.Id, ct);
        return entity is null
            ? new(false, Error: "Not Found")
            : new(true, Data: entity);
    }
}
