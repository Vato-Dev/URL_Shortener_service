using Application.Abstractions;
using Domain.Models;
using FluentValidation;
using MediatR;

namespace Application.Cqrs.ShortUrls.Commands;  // if in chache is 200 urls or every 30 min sends update request's


public abstract record ShortUrlUpdateResult
{
    public sealed record Success (Guid Id, long ClickCount) : ShortUrlUpdateResult ;
    public sealed record Failure(IEnumerable<string> Errors) : ShortUrlUpdateResult;
}

public sealed record UpdateShortUrlCommand(Guid ShortUrlId) : IRequest<ShortUrlUpdateResult>;
public class UpdateShortUrlCommandValidator : AbstractValidator<UpdateShortUrlCommand>
{
    public UpdateShortUrlCommandValidator()
    {
        RuleFor(x=>x.ShortUrlId).NotEmpty();
    } 
}

internal class UpdateShortUrlCommandHandler(IShortUrlRepository repository) : IRequestHandler<UpdateShortUrlCommand, ShortUrlUpdateResult>
{
    public async Task<ShortUrlUpdateResult> Handle(UpdateShortUrlCommand request, CancellationToken cancellationToken)
    {
       var url = await repository.GetByIdAsync(request.ShortUrlId, cancellationToken);//add chaching or concurent dict to make a batch update later
       if(url is not null && !url.IsExpired())
       {
          var count=  url.Click();
          await repository.UpdateAsync(url,cancellationToken);
         return  new ShortUrlUpdateResult.Success(url.Id, count);
       }
       return new ShortUrlUpdateResult.Failure(["NotFound"]);
    }
}

