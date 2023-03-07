﻿using System.Collections;

namespace ShortUrl.UnitTests.Data;

public class InvalidStrings : IEnumerable<object[]> {
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[] { string.Empty };
        yield return new object[] { null };
        yield return new object[] { " " };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
