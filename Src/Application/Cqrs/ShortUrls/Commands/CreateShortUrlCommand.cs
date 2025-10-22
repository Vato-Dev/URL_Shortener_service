using Application.Abstractions;
using Domain.Common;
using Domain.Models;
using FluentValidation;
using MediatR;

namespace Application.Cqrs.ShortUrls.Commands;

public abstract record ShortUrlCreationResult
{
    public sealed record Success (Guid Id, string ShortCode,string FinalUrl) : ShortUrlCreationResult ;
    public sealed record Failure(IEnumerable<string> Errors) : ShortUrlCreationResult;
}

public sealed record CreateShortUrlCommand(Guid OriginalUrlId, string? Alias) : IRequest<ShortUrlCreationResult>;

public sealed class CreateShortUrlCommandValidator: AbstractValidator<CreateShortUrlCommand>
{
    public CreateShortUrlCommandValidator(IShortUrlRepository repository)
    {
        RuleFor(x => x.OriginalUrlId).NotEmpty();
        RuleFor(x => x.Alias)
            .MaximumLength(50);
        RuleFor(x => x.Alias)
            .MaximumLength(50)
            .MustAsync(async (alias, ct) =>
            {
                if (string.IsNullOrWhiteSpace(alias))
                    return true;

                var aliasExists = await repository.IsAliasTaken(alias, ct);
                return !aliasExists;
            })
            .WithMessage("Alias is already taken");
            
    }
}

internal sealed class CreateShortUrlCommandHandler(IShortUrlRepository repository) : IRequestHandler<CreateShortUrlCommand, ShortUrlCreationResult>{
    public async Task<ShortUrlCreationResult> Handle(CreateShortUrlCommand request, CancellationToken cancellationToken)
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

        return new ShortUrlCreationResult.Success(shortUrl.Id, shortUrl.ShortUrlCode.Value, finalUrlString);
    }
}

