# BookManager

## Initial task
Develop a C# library (not Web service) that allows to create, view and edit a list of books. Each book has a title, and author, and number of pages. The library should provide following functionality:
  1. Load a list of books from an XML-file;
  2. Add a new book to the list;
  3. Sort the list in alphabetical order by author first. Then for each author sort it in alphabetical order by title. Example: first all Andersen's books, then all King's books. Andersens's books: first The Little Mermaid, then The Ugly Duckling etc.;
  4. Book search by a part of its title (basic, not fuzzy);
  5. Save the list of books into an XML-file.

The library should be implemented in C#, .NET.
No UI is required, the library will be used by other software modules.
The library shuold be covered with unit tests (any framework of your choice).

## Notes on implementation
1. The idea was to encapsulate the collection of books itself withing the library, so adding and iterating over the list are done via the `BookManager` class, editing can be performed in place on the `Book` objects themselves.
2. Defined interfaces provide extesibility points to both allow the usage out-of-the-box with default implementations of comparison and file access given in the task, and facilitate flexibility of usage by the consumer code. While not required by the task, I'd assume it's a good practice to provide such things to make maintainability easier.
3. Without more context, the base container for the books is just a `List<T>`. If sorting, for example, were a more important and frequently used part, something like a `SortedSet<T>` with a defined comparer would be a better choice. Same thing with thread safety, but as specifics were not provided I've stuck with a simple list.
4. Exceptions are mostly not handled in the library. There isn't much we can do to gracefully handle situations where source XML does not exist or is corrupted, so exception handling is left up to the consuming code.
5. Performance considerations: again, without context was not a huge concern, but in case it is, the `List<T>.Sort` should be robust enough to handle large amounts of data. Same goes for search, but in an unlikely case it's not performant enough, a strategy pattern can be implemented to use a different algorythm for large datasets (likely something similar to what full-text indexing does). Quick test on 200000 randomly generated items performs search and itertion almost instantly and sorting in around 200 ms, without any other requirements I'd consider it descent enough.
