using System.Diagnostics.Contracts;

namespace DDD.SuppleDesign.Assertions;

public class Library
{
    private List<Book> books;

    public Library()
    {
        books = new List<Book>();
    }

    public void AddBook(Book book)
    {
        // Precondition: The book should not be null
        Contract.Requires<ArgumentNullException>(book != null, "Book cannot be null.");

        // Precondition: The book should have a unique ISBN
        Contract.Requires<ArgumentException>(!ContainsBook(book.ISBN), "A book with the same ISBN already exists.");

        // Postcondition: The book should be added to the library
        Contract.Ensures(ContainsBook(book.ISBN), "Book was not added successfully.");

        books.Add(book);
    }

    public void RemoveBook(string isbn)
    {
        // Precondition: The ISBN should not be null or empty
        Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(isbn), "ISBN cannot be null or empty.");

        // Precondition: The book should exist in the library
        Contract.Requires<ArgumentException>(ContainsBook(isbn), "Book with the specified ISBN does not exist.");

        // Postcondition: The book should be removed from the library
        Contract.Ensures(!ContainsBook(isbn), "Book was not removed successfully.");

        books.RemoveAll(book => book.ISBN == isbn);
    }

    // Invariant: All books in the library should have unique ISBNs
    [ContractInvariantMethod]
    private void UniqueISBNInvariant()
    {
        Contract.Invariant(books.GroupBy(book => book.ISBN).All(group => group.Count() == 1));
    }

    private bool ContainsBook(string isbn)
    {
        return books.Any(book => book.ISBN == isbn);
    }
}

public class Book
{
    public string ISBN { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
}