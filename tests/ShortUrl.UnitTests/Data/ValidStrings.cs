using System.Collections;

namespace ShortUrl.UnitTests.Data;

public class ValidStrings : IEnumerable<object[]> {
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[] { "abc" };
        yield return new object[] { "123" };
        yield return new object[] { "a1b2c3" };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}