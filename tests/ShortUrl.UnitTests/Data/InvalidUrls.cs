using System.Collections;

namespace ShortUrl.UnitTests.Data;

public class InvalidUrls : IEnumerable<object[]> {
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[] { string.Empty };
        yield return new object[] { null };
        yield return new object[] { " " };
        yield return new object[] { "google.ru" };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
