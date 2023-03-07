namespace ShortUrl.Application.Interfaces;

public interface IQrCodeGenerator {
    byte[] Generate(string data);
}
