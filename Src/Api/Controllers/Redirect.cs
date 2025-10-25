using Application.Cqrs.HelperCommands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;


[Route("Api/{shortCode}")]
[ApiController]
public class Redirect(ISender sender) : ControllerBase
{


    [HttpGet]
    public async Task<IActionResult> RedirectToOriginal(string shortCode)
    {
        var result = await sender.Send(new RedirectByCodeToOriginalUrlCommand(shortCode));
        return result.IsSuccess ? Redirect(result.OriginalUrl!) : NotFound(result.Error);
    }
    
}