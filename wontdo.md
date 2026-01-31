# Won't Dos

This document tracks operators from [MoreLINQ](https://github.com/morelinq/MoreLINQ) that are not implemented in MoreAsyncLINQ.

## Experimental Operators

Operators that are still experimental in MoreLINQ

### Merge

Concurrently merges all the elements of multiple asynchronous streams into a single asynchronous stream.

---

### TrySingle

Returns the only element of a sequence that has just one element. If the sequence has zero or multiple elements, then returns a user-defined value that indicates the cardinality of the result sequence.

---

### Await / AwaitCompletion

Creates a sequence query that streams the result of each task in the source sequence as it completes asynchronously.

---

## Non-async Operators

These operators don't benefit from having an async version because you can either:

- Call the synchronous version and then `ToAsyncEnumerable()` (e.g. `MoreEnumerable.Sequence(1,2,3).ToAsyncEnumerable()`)
- Materialize the `IAsyncEnumerable` and then call the synchronous version (e.g. `(await asyncSource.ToListAsync()).Permutations()`)

### Return

Returns a single-element sequence containing the item provided.

---

### Sequence

Generates a sequence of numbers within the (inclusive) specified range.

---

### Random

Returns a sequence of random numbers within a specified range.

---

### Permutations

Returns a sequence of all permutations of the input sequence.

---

### Subsets

Returns a sequence representing all subsets of any size that are part of the original sequence.

---

### ToDataTable

Appends elements in the sequence as rows of a given DataTable.

---

## No Async Equivalent

These operators rely on types or interfaces that have no async counterpart.

### Flatten

Flattens a sequence containing arbitrarily-nested sequences. This operator works with the non-generic `IEnumerable` interface to handle heterogeneous nesting (e.g., `object[]` containing other `object[]`). Since there's no `IAsyncEnumerable` equivalent of the non-generic `IEnumerable`, this operator cannot be implemented for async sequences.

---

## Implemented in .NET

These operators are provided by [`System.Linq.AsyncEnumerable`](https://learn.microsoft.com/en-us/dotnet/api/system.linq.asyncenumerable?view=net-10.0) in .NET 10+.

### Append

[Returns a new sequence that contains the elements of the input sequence followed by the specified element.](https://learn.microsoft.com/en-us/dotnet/api/system.linq.asyncenumerable.append)

---

### Concat

[Concatenates two sequences.](https://learn.microsoft.com/en-us/dotnet/api/system.linq.asyncenumerable.concat)

---

### Prepend

[Returns a new sequence that contains the specified element followed by the elements of the input sequence.](https://learn.microsoft.com/en-us/dotnet/api/system.linq.asyncenumerable.prepend)

---

### Shuffle

[Shuffles the order of the elements of a sequence.](https://learn.microsoft.com/en-us/dotnet/api/system.linq.asyncenumerable.shuffle)

---

### ToHashSet

[Creates a HashSet from an async-enumerable sequence.](https://learn.microsoft.com/en-us/dotnet/api/system.linq.asyncenumerable.tohashsetasync)

---