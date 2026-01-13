# Contribution Guidelines

## `ConfigureAwait(false)` - No Longer Required

### Guidelines for This Project

Following the same pattern established by the .NET runtime:

1. **Do NOT use `ConfigureAwait(false)`** on async operations within this library
2. **Do NOT use `ConfigureAwait(false)`** on `MoveNextAsync()`, `DisposeAsync()`, or other async enumerable operations
3. Trust that consumers will configure await behavior at the call site if needed using `source.ConfigureAwait(false)`

### References:

This change was implemented in [dotnet/runtime PR #113911: Remove ConfigureAwait(false) from AsyncEnumerable LINQ](https://github.com/dotnet/runtime/pull/113911).

> As with all guidance, of course, there can be exceptions, places where it doesn’t make sense. For example, one of the larger exemptions (or at least categories that requires thought) in general-purpose libraries is when those libraries have APIs that take delegates to be invoked. In such cases, the caller of the library is passing potentially app-level code to be invoked by the library, which then effectively renders those “general purpose” assumptions of the library moot. Consider, for example, an asynchronous version of LINQ’s Where method, e.g. public static async IAsyncEnumerable WhereAsync(this IAsyncEnumerable source, Func<T, bool> predicate). Does predicate here need to be invoked back on the original SynchronizationContext of the caller? That’s up to the implementation of WhereAsync to decide, and it’s a reason it may choose not to use ConfigureAwait(false).

- [ConfigureAwait FAQ by Stephen Toub](https://devblogs.microsoft.com/dotnet/configureawait-faq/)