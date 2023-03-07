using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShortUrl.Application.Interfaces;

namespace ShortUrl.Api.Controllers;

[ApiController]
public class QrCodesController : ControllerBase {
    private readonly IApplicationContext _context;
    private readonly IQrCodeGenerator _qrCodeGenerator;

    public QrCodesController(IApplicationContext context, IQrCodeGenerator qrCodeGenerator)
    {
        _context = context;
        _qrCodeGenerator = qrCodeGenerator;
    }

    [HttpGet("/qr/{token}")]
    public async Task<IActionResult> GenerateQrCode(string token)
    {
        var shortLink = await _context.ShortLinks.FirstOrDefaultAsync(x => x.Token == token);
        if (shortLink is null) return NotFound();

        byte[] qr = _qrCodeGenerator.Generate(token);
        return new FileContentResult(qr, "image/png");
    }
}
