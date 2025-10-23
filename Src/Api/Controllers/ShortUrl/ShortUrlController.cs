using Application.Cqrs.RegularUrls;
using Application.Cqrs.ShortUrls.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.ShortUrl;

[Route("api/shortUrl")]
[ApiController]
public class ShortUrlController(ISender sender) : ControllerBase
{

    [HttpPost("Create")]
    public async Task<IActionResult> CreateShortUrl([FromBody] CreateShortUrlRequest request)
    {
        var originalUrl = await sender.Send(new CreateRegularUrlCommand(request.OriginalUrlString));
        var shortUrl = await sender.Send(new CreateShortUrlCommand(originalUrl.Id, request.Alias));
        
        return shortUrl is ShortUrlCreationResult.Success ? Ok(shortUrl) : BadRequest(shortUrl);
    }

    [HttpPost("Update")]
    public async Task<IActionResult> UpdateShortUrl([FromBody] Guid id)
    {
        var result = await sender.Send(new UpdateShortUrlCommand(id));
        return result is ShortUrlUpdateResult.Success ? Ok(result) : BadRequest(result);
    }
}

public record CreateShortUrlRequest(string? Alias, string OriginalUrlString);