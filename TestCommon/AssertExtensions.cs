using Xunit;

namespace TestCommon;

public static class AssertExtensions
{
    public static void SequenceEquals<T>(IEnumerable<T> expected, IEnumerable<T> actual) => Assert.True(actual.SequenceEqual(expected));
}
