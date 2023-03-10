using System.Net;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using ShortUrl.Api.Dto;
using ShortUrl.Api.Options;
using ShortUrl.Application.Interfaces;

namespace ShortUrl.Api.Controllers.v1;

[ApiController]
[Route("api/v{api:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class TokenController : ControllerBase {
    private readonly IUrlShortener _urlShortener;
    private readonly IApplicationContext _context;
    private readonly IValidator<CreateShortLinkDto> _validator;
    private readonly IDistributedCache _cache;
    private readonly CacheOptions _cacheOptions;

    public TokenController(IUrlShortener urlShortener, IApplicationContext context,
        IValidator<CreateShortLinkDto> validator, IDistributedCache cache, IOptions<CacheOptions> cacheOptions)
    {
        _urlShortener = urlShortener;
        _context = context;
        _validator = validator;
        _cache = cache;
        _cacheOptions = cacheOptions.Value;
    }

    [HttpPost]
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
    [ResponseCache(VaryByHeader = "User-Agent", Duration = 60)]
    [HttpGet("{token}")]
    public async Task<IActionResult> RedirectToOriginalUrl(string token)
    {
        if (_cacheOptions.CacheTokens)
        {
            string? cachedUrl = await _cache.GetStringAsync($"url_{token}");
            if (!string.IsNullOrWhiteSpace(cachedUrl)) return Redirect(cachedUrl);
        }

        var shortLink = await _context.ShortLinks.FirstOrDefaultAsync(x => x.Token == token);
        if (shortLink is null) return NotFound();

        if (_cacheOptions.CacheTokens)
        {
            await _cache.SetStringAsync($"url_{token}", shortLink.OriginalUrl,
                new DistributedCacheEntryOptions().SetSlidingExpiration(
                    TimeSpan.FromMinutes(_cacheOptions.CacheTokensTime)));
        }

        return Redirect(shortLink.OriginalUrl);
    }
}
