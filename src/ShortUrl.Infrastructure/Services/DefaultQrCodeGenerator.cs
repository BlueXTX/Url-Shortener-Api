using QRCoder;
using ShortUrl.Application.Interfaces;

namespace ShortUrl.Infrastructure.Services;

public class DefaultQrCodeGenerator : IQrCodeGenerator {

    private readonly QRCodeGenerator _generator = new();

    public byte[] Generate(string data)
    {
        if (string.IsNullOrWhiteSpace(data))
            throw new ArgumentException($"Can't generate qr code for string \"{data}\"");

        var qrCodeData = _generator.CreateQrCode(data, QRCodeGenerator.ECCLevel.L);
        var qrCode = new PngByteQRCode(qrCodeData);
        return qrCode.GetGraphic(20);
    }
}
