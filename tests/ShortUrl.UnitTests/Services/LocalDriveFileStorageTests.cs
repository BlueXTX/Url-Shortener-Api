using System.Security.Cryptography;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using ShortUrl.Infrastructure.Options;
using ShortUrl.Infrastructure.Services;
using ShortUrl.UnitTests.Data;

namespace ShortUrl.UnitTests.Services;

public class LocalDriveFileStorageTests {
    private const string BasePath = "/images";
    private static readonly LocalDriveFileStorageOptions Options = new(BasePath);

    private static string GetFilePath(string fileName) => Options.UseRelativePath
        ? Path.Join(AppContext.BaseDirectory, BasePath, fileName)
        : Path.Join(BasePath, fileName);

    private static LocalDriveFileStorage CreateFileStorage()
    {
        var mock = new Mock<IOptionsSnapshot<LocalDriveFileStorageOptions>>();
        mock.Setup(m => m.Value).Returns(Options);

        return new LocalDriveFileStorage(mock.Object);
    }

    [Theory]
    [ClassData(typeof(ValidFileSizes))]
    private async Task Write_WithFileSize_FileShouldBeCreated(int fileSize)
    {
        var fileStorage = CreateFileStorage();
        string fileName = Guid.NewGuid().ToString();
        byte[] bytes = new byte[fileSize];

        await fileStorage.Write(fileName, new MemoryStream(bytes));

        var file = File.OpenRead(GetFilePath(fileName));

        file.Length.Should().Be(fileSize);
    }

    [Theory]
    [ClassData(typeof(ValidFileSizes))]
    private async Task Read_AfterWrite_BytesShouldBeTheSame(int fileSize)
    {
        var fileStorage = CreateFileStorage();
        string fileName = Guid.NewGuid().ToString();
        byte[] bytes = new byte[fileSize];
        using var writeStream = new MemoryStream(bytes);

        await fileStorage.Write(fileName, writeStream);

        var stream = await fileStorage.Read(fileName);
        using var memoryStream = new MemoryStream();
        await stream.CopyToAsync(memoryStream);
        byte[] readBytes = memoryStream.ToArray();

        readBytes.Should().BeEquivalentTo(bytes);
    }

    [Theory]
    [ClassData(typeof(ValidFileSizes))]
    private async Task Delete_AfterWrite_FileShouldBeDeleted(int fileSize)
    {
        var fileStorage = CreateFileStorage();
        string fileName = Guid.NewGuid().ToString();
        byte[] bytes = new byte[fileSize];

        await fileStorage.Write(fileName, new MemoryStream(bytes));
        await fileStorage.Delete(fileName);
        bool actual = File.Exists(GetFilePath(fileName));
        actual.Should().BeFalse();
    }
}
