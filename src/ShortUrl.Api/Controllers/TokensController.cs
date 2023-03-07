using System.Net;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShortUrl.Api.Dto;
using ShortUrl.Application.Interfaces;

namespace ShortUrl.Api.Controllers;

[ApiController]
[Route("/")]
public class TokensController : ControllerBase {
    private readonly IUrlShortener _urlShortener;
    private readonly IApplicationContext _context;
    private readonly IValidator<CreateShortLinkDto> _validator;

    public TokensController(IUrlShortener urlShortener, IApplicationContext context,
        IValidator<CreateShortLinkDto> validator)
    {
        _urlShortener = urlShortener;
        _context = context;
        _validator = validator;
    }

    [HttpPost("/")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<ActionResult<ShortLinkDto>> CreateShortLink([FromBody] CreateShortLinkDto dto)
    {
        var validationResult = await _validator.ValidateAsync(dto);

        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            return ValidationProblem(ModelState);
        }

        var shortLink = await _urlShortener.ShortenUrl(dto.OriginalUrl);
        return Ok(new ShortLinkDto(shortLink.OriginalUrl, shortLink.Token));
    }

    [ProducesResponseType((int)HttpStatusCode.Redirect)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [HttpGet("{token}")]
    public async Task<IActionResult> RedirectToOriginalUrl(string token)
    {
        var shortLink = await _context.ShortLinks.FirstOrDefaultAsync(x => x.Token == token);
        if (shortLink is null) return NotFound();

        return Redirect(shortLink.OriginalUrl);
    }
}
