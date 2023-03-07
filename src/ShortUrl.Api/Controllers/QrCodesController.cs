using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShortUrl.Application.Interfaces;

namespace ShortUrl.Api.Controllers;

[ApiController]
public class QrCodesController : ControllerBase {
    private readonly IApplicationContext _context;
    private readonly IQrCodeGenerator _qrCodeGenerator;
    private readonly IFileStorage _fileStorage;

    public QrCodesController(IApplicationContext context, IQrCodeGenerator qrCodeGenerator, IFileStorage fileStorage)
    {
        _context = context;
        _qrCodeGenerator = qrCodeGenerator;
        _fileStorage = fileStorage;
    }

    [HttpGet("/qr/{token}")]
    public async Task<IActionResult> GenerateQrCode(string token)
    {
        var shortLink = await _context.ShortLinks.FirstOrDefaultAsync(x => x.Token == token);
        if (shortLink is null) return NotFound();

        var readStream = await _fileStorage.Read(token);
        if (readStream != Stream.Null && readStream.Length > 0) return new FileStreamResult(readStream, "image/png");

        byte[] qr = _qrCodeGenerator.Generate(token);
        using var writeStream = new MemoryStream(qr);
        await _fileStorage.Write(token + ".png", writeStream);

        return new FileContentResult(qr, "image/png");
    }
}
