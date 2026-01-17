# Design Guidelines

## Naming

- **Fluent operators** (return `IAsyncEnumerable<T>`) — no suffix (e.g., `Pipe`, `Choose`, `Batch`)
- **Materializing operators** (return `ValueTask<T>`) — `Async` suffix (e.g., `FoldAsync`, `ConsumeAsync`, `StartsWithAsync`)

## Overloads

Each operator has sync and async delegate overloads sharing the same method name:
- Sync: `Func<T, TResult>`
- Async: `Func<T, CancellationToken, ValueTask<TResult>>` — `CancellationToken` is always the last delegate parameter

## Implementation

- Use `IsKnownEmpty()` to short-circuit known empty sequences
- Validate arguments in the public method, then delegate to a `static` local `Core` method
- Use `[EnumeratorCancellation]` on the `CancellationToken` parameter in iterator methods
- Use `source.WithCancellation(cancellationToken)` in `await foreach` loops
- No `ConfigureAwait(false)` — follow [.NET runtime conventions](https://github.com/dotnet/runtime/pull/113911)
