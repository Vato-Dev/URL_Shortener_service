using System.Collections;
using Application.Abstractions;
using Domain.Models;
using FluentValidation;
using MediatR;

namespace Application.Cqrs.RegularUrls;

public sealed class CreateRegularUrlCommand(string urlString) : IRequest<RegularUrl>
{
    public string UrlString { get; init; } = urlString;
}

public sealed class CreateRegularUrlCommandValidator : AbstractValidator<CreateRegularUrlCommand>
{
    public CreateRegularUrlCommandValidator()
    {
        
        RuleFor(x => x.UrlString)
            .NotEmpty().WithMessage("URL cannot be empty.")
            .MinimumLength(5).WithMessage("Minimum length is 5 characters.")
            .MaximumLength(2048).WithMessage("Invalid URL.")
            .Must(url => Uri.TryCreate(url, UriKind.Absolute, out _))
            .WithMessage("Invalid URL.");
    }
}

internal sealed class CreateRegularUrlCommandHandler(IRegularUrlRepository repository)
    : IRequestHandler<CreateRegularUrlCommand, RegularUrl>
{
    public async Task<RegularUrl> Handle(CreateRegularUrlCommand request, CancellationToken cancellationToken)
    {
        var existingUrl = await repository.GetByUrlStringAsync(request.UrlString, cancellationToken);
        if (existingUrl is not null)
        {
            return existingUrl;
        }
        var urlOriginal = new RegularUrl(request.UrlString);
        await repository.CreateAsync(urlOriginal, cancellationToken);
        return urlOriginal;
    }
}