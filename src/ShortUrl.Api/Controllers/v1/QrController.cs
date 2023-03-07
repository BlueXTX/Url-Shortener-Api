using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using ShortUrl.Api.Options;
using ShortUrl.Application.Interfaces;
using ShortUrl.Infrastructure.Options;

namespace ShortUrl.Api.Controllers.v1;

[ApiController]
[Route("api/v{api:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class QrController : ControllerBase {
    private readonly IApplicationContext _context;
    private readonly IQrCodeGenerator _qrCodeGenerator;
    private readonly QrCodeGenerationOptions _qrCodeGenerationOptions;
    private readonly IFileStorage _fileStorage;
    private readonly IDistributedCache _cache;
    private readonly CacheOptions _cacheOptions;

    public QrController(IApplicationContext context, IQrCodeGenerator qrCodeGenerator, IFileStorage fileStorage,
        IDistributedCache cache, IOptions<QrCodeGenerationOptions> qrCodeGeneratorOptions,
        IOptions<CacheOptions> cacheOptions)
    {
        _context = context;
        _qrCodeGenerator = qrCodeGenerator;
        _qrCodeGenerationOptions = qrCodeGeneratorOptions.Value;
        _fileStorage = fileStorage;
        _cache = cache;
        _cacheOptions = cacheOptions.Value;
    }

    [HttpGet("{token}")]
    [ResponseCache(VaryByHeader = "User-Agent", Duration = 60)]
    public async Task<IActionResult> GenerateQrCode(string token)
    {
        if (_cacheOptions.CacheImages)
        {
            byte[]? cachedQr = await _cache.GetAsync($"qr_{token}");
            if (cachedQr is not null) return new FileContentResult(cachedQr, "image/png");
        }

        var shortLink = await _context.ShortLinks.FirstOrDefaultAsync(x => x.Token == token);
        if (shortLink is null) return NotFound();

        if (_cacheOptions.SaveImagesToDrive)
        {
            await using var readStream = await _fileStorage.Read(token);
            if (readStream != Stream.Null && readStream.Length > 0)
                return new FileStreamResult(readStream, "image/png");
        }


        byte[] qr = _qrCodeGenerator.Generate(_qrCodeGenerationOptions.BaseUrl + "/" + token);
        using var writeStream = new MemoryStream(qr);
        
        if (_cacheOptions.SaveImagesToDrive) await _fileStorage.Write(token + ".png", writeStream);

        if (_cacheOptions.CacheImages)
        {
            await _cache.SetAsync($"qr_{token}", qr,
                new DistributedCacheEntryOptions().SetSlidingExpiration(
                    TimeSpan.FromMinutes(_cacheOptions.CacheImagesTime)));
        }

        return new FileContentResult(qr, "image/png");
    }
}
