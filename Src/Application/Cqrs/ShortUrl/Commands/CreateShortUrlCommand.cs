using Application.Abstractions;
using FluentValidation;
using MediatR;

namespace Application.Cqrs.ShortUrl.Commands;

public sealed record CreateShortUrlCommand : IRequest<Domain.ShortUrl>
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
            .MustAsync(async (alias, ct) =>
            {
                var aliasExists = await repository.ExistsByAliasAsync(alias!, ct);
                return !aliasExists;
            }).When(x => x.Alias is not null).WithMessage($"Alias is already taken");
    }
}

internal sealed class CreateShortUrlCommandHandler : IRequestHandler<CreateShortUrlCommand, Domain.ShortUrl>{
    public Task<Domain.ShortUrl> Handle(CreateShortUrlCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}