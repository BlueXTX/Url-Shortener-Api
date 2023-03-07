using System.Collections;

namespace ShortUrl.UnitTests.Data;

public class ValidUrls : IEnumerable<object[]> {
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[] { "https://google.com" };
        yield return new object[] { "http://google.com" };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
