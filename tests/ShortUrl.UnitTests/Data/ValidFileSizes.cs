using System.Collections;

namespace ShortUrl.UnitTests.Data;

public class ValidFileSizes : IEnumerable<object[]> {
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[] { 2 };
        yield return new object[] { 64 };
        yield return new object[] { 512 };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}