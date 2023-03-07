using System.Collections;

namespace ShortUrl.UnitTests.Data;

public class TestNumbers : IEnumerable<object[]> {
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[] { 0 };
        yield return new object[] { 1 };
        yield return new object[] { 2 };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}