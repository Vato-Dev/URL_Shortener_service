using Application.Abstractions;
using MediatR;

namespace Application.Cqrs.HelperCommands;

public sealed record RedirectByCodeToOriginalUrlCommand(string Code)
    : IRequest<RecordRedirectByCodeResult>;

public sealed record RecordRedirectByCodeResult(
    bool IsSuccess,
    string? OriginalUrl = null,
    string? Error = null);


internal class RedirectByCodeToOriginalUrlCommandHandler(IShortUrlRepository shortUrlRepository, IRegularUrlRepository regularUrlRepository) 
    : IRequestHandler<RedirectByCodeToOriginalUrlCommand, RecordRedirectByCodeResult>
{
    public async Task<RecordRedirectByCodeResult> Handle(RedirectByCodeToOriginalUrlCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Code) || request.Code.Length >50 )
            return new RecordRedirectByCodeResult(false, null, "Invalid Input");
        
        var regularUrlId = await shortUrlRepository.GetOriginalUrlIdByAliasOrCode(request.Code);
      
      var url =  await regularUrlRepository.GetByIdAsync(regularUrlId,cancellationToken);
      
      return url is null ? new RecordRedirectByCodeResult(IsSuccess:false, OriginalUrl: null , Error: "Not Found") : new RecordRedirectByCodeResult(IsSuccess: true , OriginalUrl: url.UrlString, Error: null);
    }
}