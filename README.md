# MoreAsyncLINQ
[![NuGet](https://img.shields.io/nuget/dt/MoreAsyncLINQ.svg)](https://www.nuget.org/packages/MoreAsyncLINQ)
[![NuGet](https://img.shields.io/nuget/v/MoreAsyncLINQ.svg)](https://www.nuget.org/packages/MoreAsyncLINQ)
[![license](https://img.shields.io/github/license/i3arnon/MoreAsyncLINQ.svg)](LICENSE)

Additional async `LINQ` to objects operators for `IAsyncEnumerable<T>`

It's like [MoreLINQ], only asynchronous.

## Operators

### Acquire

Ensures that a source sequence of disposable objects are all acquired
successfully. If the acquisition of any one fails then those successfully
acquired till that point are disposed.

### Aggregate

Applies multiple accumulators sequentially in a single pass over a sequence.

### AggregateRight

Applies a right-associative accumulator function over a sequence.
This operator is the right-associative version of the `Aggregate` operator.

### Append

Returns a sequence consisting of the head element and the given tail elements.

### Assert

Asserts that all elements of a sequence meet a given condition otherwise
throws an exception.

### AssertCount

Asserts that a source sequence contains a given count of elements.

### AtLeast

Determines whether or not the number of elements in the sequence is greater
than or equal to the given integer.

### AtMost

Determines whether or not the number of elements in the sequence is lesser
than or equal to the given integer.

### Backsert

Inserts the elements of a sequence into another sequence at a
specified index from the tail of the sequence, where zero always represents
the last position, one represents the second-last element, two represents
the third-last element and so on.

### Batch

Batches the source sequence into sized buckets.

### Cartesian

Returns the Cartesian product of two or more sequences by combining each
element from the sequences and applying a user-defined projection to the
set.

### Choose

Applies a function to each element of the source sequence and returns a new
sequence of result elements for source elements where the function returns a
couple (2-tuple) having a `true` as its first element and result as the
second.

### CompareCount

Compares two sequences and returns an integer that indicates whether the
first sequence has fewer, the same or more elements than the second sequence.

### Consume

Completely consumes the given sequence. This method uses immediate execution,
and doesn't store any data during execution

### CountBetween

Determines whether or not the number of elements in the sequence is between an
inclusive range of minimum and maximum integers.

### CountBy

Applies a key-generating function to each element of a sequence and returns a
sequence of unique keys and their number of occurrences in the original
sequence.

### CountDown

Provides a countdown counter for a given count of elements at the tail of the
sequence where zero always represents the last element, one represents the
second-last element, two represents the third-last element and so on.

### DistinctBy

Returns all distinct elements of the given source, where "distinctness" is
determined via a projection and the default equality comparer for the
projected type.

### EndsWith

Determines whether the end of the first sequence is equivalent to the second
sequence.

### EquiZip

Returns a projection of tuples, where each tuple contains the N-th
element from each of the argument sequences. An exception is thrown
if the input sequences are of different lengths.

### Evaluate
Returns a sequence containing the values resulting from invoking (in order) each function in the source sequence of functions.

### Exactly

Determines whether or not the number of elements in the sequence is equals
to the given integer.

### ExceptBy

Returns the set of elements in the first sequence which aren't in the second
sequence, according to a given key selector.

### Exclude

Excludes elements from a sequence starting at a given index

### FallbackIfEmpty

Returns the elements of a sequence and falls back to another if the original
sequence is empty.

### FillBackward

Returns a sequence with each null reference or value in the source replaced
with the following non-null reference or value in that sequence.

### FillForward

Returns a sequence with each null reference or value in the source replaced
with the previous non-null reference or value seen in that sequence.

### Flatten

Flattens a sequence containing arbitrarily-nested sequences.

### Fold

Returns the result of applying a function to a sequence with 1 to 16 elements.

### ForEach

Immediately executes the given action on each element in the source sequence.

### From

Returns a sequence containing the values resulting from invoking (in order)
each function in the source sequence of functions.

### FullGroupJoin

Performs a Full Group Join between the and sequences.

### FullJoin

Performs a full outer join between two sequences.

### Generate

Returns a sequence of values consecutively generated by a generator function

### GenerateByIndex

Returns a sequence of values based on indexes

### GroupAdjacent

Groups the adjacent elements of a sequence according to a specified key
selector function.

### Index

Returns a sequence of tuples of an element and its zero-based index in the source sequence.

### IndexBy


Applies a key-generating function to each element of a sequence and returns
a sequence that contains the elements of the original sequence as well its
key and index inside the group of its key. An additional argument specifies
a comparer to use for testing equivalence of keys.

### Insert

Inserts the elements of a sequence into another sequence at a specified index.

### Interleave

Interleaves the elements of two or more sequences into a single sequence,
skipping sequences as they are consumed.

### Lag

Produces a projection of a sequence by evaluating pairs of elements separated
by a negative offset.

### Lead

Produces a projection of a sequence by evaluating pairs of elements separated
by a positive offset.

### LeftJoin

Performs a left outer join between two sequences.

### MaxBy

Returns the maxima (maximal elements) of the given sequence, based on the
given projection.

### MinBy

Returns the minima (minimal elements) of the given sequence, based on the
given projection.

### Move

Returns a sequence with a range of elements in the source sequence
moved to a new offset.

### OrderBy

Sorts the elements of a sequence in a particular direction (ascending,
descending) according to a key.

### OrderedMerge

Merges two ordered sequences into one. Where the elements equal in both
sequences, the element from the first sequence is returned in the resulting
sequence.

### Pad

Pads a sequence with default values if it is narrower (shorter in length) than
a given width.

### PadStart

Pads a sequence with default values in the beginning if it is narrower
(shorter in length) than a given width.

### Pairwise

Returns a sequence resulting from applying a function to each element in the
source sequence and its predecessor, with the exception of the first element
which is only returned as the predecessor of the second element

### PartialSort

Combines `OrderBy` (where element is key) and `Take` in a single operation.

### PartialSortBy

Combines `OrderBy` and `Take` in a single operation.

### Partition

Partitions a sequence by a predicate, or a grouping by Boolean keys or up to 3
sets of keys.

### Pipe

Executes the given action on each element in the source sequence and yields it

### Prepend

Prepends a single value to a sequence

### PreScan

Performs a pre-scan (exclusive prefix sum) on a sequence of elements

### Rank

Ranks each item in the sequence in descending ordering using a default
comparer.

### RankBy

Ranks each item in the sequence in descending ordering by a specified key
using a default comparer.

### Repeat

Repeats the sequence indefinitely or a specific number of times.

### RightJoin

Performs a right outer join between two sequences.

### RunLengthEncode

Run-length encodes a sequence by converting consecutive instances of the same
element into a `KeyValuePair<T, int>` representing the item and its occurrence
count.

### Scan

Performs a scan (inclusive prefix sum) on a sequence of elements.

### ScanBy

Applies an accumulator function over sequence element keys, returning the keys
along with intermediate accumulator states.

### ScanRight

Performs a right-associative scan (inclusive prefix) on a sequence of elements.
This operator is the right-associative version of the Scan operator.

### Segment

Divides a sequence into multiple sequences by using a segment detector based
on the original sequence.

### SkipLast

Bypasses a specified number of elements at the end of the sequence.

### SkipUntil

Skips items from the input sequence until the given predicate returns true
when applied to the current source item; that item will be the last skipped

### Slice

Extracts elements from a sequence at a particular zero-based starting index

### SortedMerge

Merges two or more sequences that are in a common order (either ascending or
descending) into a single sequence that preserves that order.

### Split

Splits the source sequence by a separator.

### StartsWith

Determines whether the beginning of the first sequence is equivalent to the
second sequence.

### TagFirstLast

Returns a sequence resulting from applying a function to each element in the
source sequence with additional parameters indicating whether the element is
the first and/or last of the sequence

### TakeEvery

Returns every N-th element of a source sequence

### TakeLast

Returns a specified number of contiguous elements from the end of a sequence

### TakeUntil

Returns items from the input sequence until the given predicate returns true
when applied to the current source item; that item will be the last returned

### ThenBy

Performs a subsequent ordering of elements in a sequence in a particular
direction (ascending, descending) according to a key.

### ToArrayByIndex

Creates an array from an IEnumerable<T> where a function is used to determine
the index at which an element will be placed in the array.

### ToDelimitedString

Creates a delimited string from a sequence of values. The delimiter used
depends on the current culture of the executing thread.

### ToDictionary

Creates a [dictionary][dict] from a sequence of [key-value pair][kvp] elements
or tuples of 2.

### ToLookup

Creates a [lookup][lookup] from a sequence of [key-value pair][kvp] elements
or tuples of 2.

### Trace

Traces the elements of a source sequence for diagnostics.

### Transpose

Transposes the rows of a sequence into columns.

### TraverseBreadthFirst

Traverses a tree in a breadth-first fashion, starting at a root node and using
a user-defined function to get the children at each node of the tree.

### TraverseDepthFirst

Traverses a tree in a depth-first fashion, starting at a root node and using a
user-defined function to get the children at each node of the tree.

### Unfold

Returns a sequence generated by applying a state to the generator function,
and from its result, determines if the sequence should have a next element and
its value, and the next state in the recursive call.

### Window

Processes a sequence into a series of subsequences representing a windowed
subset of the original

### WindowLeft

Creates a left-aligned sliding window over the source sequence of a given size.

### WindowRight

Creates a right-aligned sliding window over the source sequence of a given size.

### ZipLongest

Returns a projection of tuples, where each tuple contains the N-th
element from each of the argument sequences. The resulting sequence
will always be as long as the longest of input sequences where the
default value of each of the shorter sequence element types is used
for padding.

### ZipShortest

Returns a projection of tuples, where each tuple contains the N-th
element from each of the argument sequences. The resulting sequence
is as short as the shortest input sequence.

---
Credit: This project and its documentation are derived from [MoreLINQ] 

[MoreLINQ]: https://github.com/morelinq/MoreLINQ/
[dict]: https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Generic.Dictionary-2
[kvp]: https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Generic.KeyValuePair-2
[lookup]: https://docs.microsoft.com/en-us/dotnet/api/system.linq.lookup-2