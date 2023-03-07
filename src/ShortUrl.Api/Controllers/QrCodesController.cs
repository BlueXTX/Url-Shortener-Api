using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using ShortUrl.Application.Interfaces;
using ShortUrl.Infrastructure.Options;

namespace ShortUrl.Api.Controllers;

[ApiController]
public class QrCodesController : ControllerBase {
    private readonly IApplicationContext _context;
    private readonly IQrCodeGenerator _qrCodeGenerator;
    private readonly QrCodeGenerationOptions _qrCodeGenerationOptions;
    private readonly IFileStorage _fileStorage;
    private readonly IDistributedCache _cache;

    private static readonly DistributedCacheEntryOptions CacheEntryOptions =
        new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(5));

    public QrCodesController(IApplicationContext context, IQrCodeGenerator qrCodeGenerator, IFileStorage fileStorage,
        IDistributedCache cache, IOptions<QrCodeGenerationOptions> qrCodeGeneratorOptions)
    {
        _context = context;
        _qrCodeGenerator = qrCodeGenerator;
        _qrCodeGenerationOptions = qrCodeGeneratorOptions.Value;
        _fileStorage = fileStorage;
        _cache = cache;
    }

    [HttpGet("/qr/{token}")]
    [ResponseCache(VaryByHeader = "User-Agent", Duration = 60)]
    public async Task<IActionResult> GenerateQrCode(string token)
    {
        byte[]? cachedQr = await _cache.GetAsync($"qr_{token}");
        if (cachedQr is not null) return new FileContentResult(cachedQr, "image/png");

        var shortLink = await _context.ShortLinks.FirstOrDefaultAsync(x => x.Token == token);
        if (shortLink is null) return NotFound();

        await using var readStream = await _fileStorage.Read(token);
        if (readStream != Stream.Null && readStream.Length > 0)
            return new FileStreamResult(readStream, "image/png");

        byte[] qr = _qrCodeGenerator.Generate(_qrCodeGenerationOptions.BaseUrl + "/" + token);
        using var writeStream = new MemoryStream(qr);
        await _fileStorage.Write(token + ".png", writeStream);

        await _cache.SetAsync($"qr_{token}", qr, CacheEntryOptions);

        return new FileContentResult(qr, "image/png");
    }
}
