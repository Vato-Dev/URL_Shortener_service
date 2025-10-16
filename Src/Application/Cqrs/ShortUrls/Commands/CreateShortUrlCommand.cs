using Application.Abstractions;
using Domain.Common;
using Domain.Models;
using FluentValidation;
using MediatR;

namespace Application.Cqrs.ShortUrls.Commands;
public sealed record ShortUrlDto(
    Guid Id, 
    string ShortCode, 
    string FinalUrl
);
public sealed record CreateShortUrlCommand : IRequest<ShortUrlDto>
{
    public Guid OriginalUrlId { get; init; } 
    public string? Alias { get; init; }
}

public sealed class CreateShortUrlCommandValidator: AbstractValidator<CreateShortUrlCommand>
{
    public CreateShortUrlCommandValidator(IShortUrlRepository repository)
    {
        RuleFor(x => x.OriginalUrlId).NotEmpty();
        RuleFor(x => x.Alias)
            .MaximumLength(50);
        RuleFor(x => x.Alias)
            .MustAsync(async (alias, ct) =>
            {
                var aliasExists = await repository.IsAliasTaken(alias!, ct);
                return !aliasExists;
            }).When(x => x.Alias is not null).WithMessage($"Alias is already taken");
    }
}

internal sealed class CreateShortUrlCommandHandler(IShortUrlRepository repository) : IRequestHandler<CreateShortUrlCommand, ShortUrlDto>{
    public async Task<ShortUrlDto> Handle(CreateShortUrlCommand request, CancellationToken cancellationToken)
    {
        var schemeName = "URL__SCHEME_NAME".FromEnv() ?? "https"; 
        var hostName = "URL__Host_Name".FromEnvRequired();
        
        var shortUrl = ShortUrl.Create(request.Alias, request.OriginalUrlId);
        
        await repository.CreateAsync(shortUrl, cancellationToken);
        
        var urlPathSegment = shortUrl.HasAlias ? shortUrl.Alias : shortUrl.ShortUrlCode.Value;
        
        var finalUrlString = new UriBuilder(schemeName, hostName) 
        { 
            Path = urlPathSegment 
        }.ToString();

        return new ShortUrlDto(
            shortUrl.Id, 
            shortUrl.ShortUrlCode.Value, 
            finalUrlString
        );
    }
}

