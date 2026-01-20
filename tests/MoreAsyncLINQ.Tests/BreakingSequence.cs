namespace MoreAsyncLINQ.Tests;

public sealed class BreakingSequence<T> : IAsyncEnumerable<T>
{
    public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken) =>
        throw new InvalidOperationException("Sequence should not be enumerated.");
}