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

## Tests

- Inherit from `AsyncEnumerableTests`
- Standard test methods:
  - `InvalidInputs_Throws` — verify `ArgumentNullException` for all null inputs
  - `EmptySequence` — use `AssertKnownEmpty()` to verify empty sequence optimization
  - `IsLazy` — use `BreakingSequence<T>` and `BreakingFunc.Of`/`OfAsync` to verify deferred execution
- Use `[Theory]` with `[MemberData(nameof(IsAsync))]` to test sync and async delegate overloads
- Use `AssertEqual(syncMoreLinqResult, asyncResult)` to verify behavior matches MoreLINQ
