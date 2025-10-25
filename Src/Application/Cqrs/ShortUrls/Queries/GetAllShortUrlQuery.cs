using Application.Abstractions;
using Domain.Models;
using MediatR;

namespace Application.Cqrs.ShortUrls.Queries;

public sealed record GetAllShortUrlQuery : IRequest<GetAllShortUrlResult>;

public sealed record GetAllShortUrlResult(
    bool IsSuccess,
    List<ShortUrl>? ShortUrls = null,
    string? Error = null
);

internal class GetAllShortUrlQueryHandler(IShortUrlRepository repository)
    : IRequestHandler<GetAllShortUrlQuery, GetAllShortUrlResult>
{
    public async Task<GetAllShortUrlResult> Handle(GetAllShortUrlQuery request, CancellationToken ct)
    {
        var results = (await repository.GetAllAsync(ct)).ToList();

        return results.Count > 0
            ? new GetAllShortUrlResult(true, results)
            : new GetAllShortUrlResult(false, Error: "Not Found");
    }
}