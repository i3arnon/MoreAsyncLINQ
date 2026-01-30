# Won't Dos

This document tracks operators from [MoreLINQ](https://github.com/morelinq/MoreLINQ) that are not yet implemented in MoreAsyncLINQ.

Operators are ordered by estimated importance/popularity for async scenarios.

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