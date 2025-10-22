// using Application.Abstractions;
// using Domain.Models;
// using FluentValidation;
// using MediatR;
//
// namespace Application.Cqrs.ShortUrls.Commands;  // if in chache is 200 urls or every 30 min sends update request's
//
// public class UpdateShortUrlCommand : IRequest<ShortUrl>
// {
//     public required Guid ShortUrlId { get; init; }
// }
//
// public class UpdateShortUrlCommandValidator : AbstractValidator<UpdateShortUrlCommand>
// {
//     public UpdateShortUrlCommandValidator()
//     {
//         RuleFor(x=>x.ShortUrlId).NotEmpty();
//     } 
// }
//
// internal class UpdateShortUrlCommandHandler(IShortUrlRepository repository) : IRequestHandler<UpdateShortUrlCommand, ShortUrl>
// {
//     public Task<ShortUrl> Handle(UpdateShortUrlCommand request, CancellationToken cancellationToken)
//     {
//        var url = repository.GetByIdAsync(request.ShortUrlId, cancellationToken);// i think because url is not existing creating new is wrong cuz we don't know which original url is owner
//        if (url == null)
//        {
//            throw new Exception("Shorted Url not found");
//        }
//        url.
//     }
// }