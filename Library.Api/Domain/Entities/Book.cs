namespace Library.Api.Domain.Entities;

public class Book
{
    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public string Author { get; private set; }
    public string Isbn { get; private set; }
    public int PublishedYear { get; private set; }
    public int TotalCopies { get; private set; }
    public int AvailableCopies { get; private set; }

    public Book(string title, string author, string isbn, int publishedYear, int totalCopies)
    {
        if (publishedYear > DateTime.UtcNow.Year)
            throw new ArgumentException("Published year cannot be in the future.");
        if (totalCopies <= 0)
            throw new ArgumentException("Total copies must be greater than 0.");

        Id = Guid.NewGuid();
        Title = title;
        Author = author;
        Isbn = isbn;
        PublishedYear = publishedYear;
        TotalCopies = totalCopies;
        AvailableCopies = totalCopies;
    }

    public void Update(string title, string author, int publishedYear, int totalCopies)
    {
        if (publishedYear > DateTime.UtcNow.Year)
            throw new ArgumentException("Published year cannot be in the future.");
        if (totalCopies < TotalCopies - AvailableCopies)
            throw new InvalidOperationException("Cannot reduce total copies below currently borrowed count.");

        var borrowed = TotalCopies - AvailableCopies;
        Title = title;
        Author = author;
        PublishedYear = publishedYear;
        TotalCopies = totalCopies;
        AvailableCopies = totalCopies - borrowed;
    }

    public void BorrowCopy()
    {
        if (AvailableCopies <= 0)
            throw new InvalidOperationException("No available copies to borrow.");
        AvailableCopies--;
    }

    public void ReturnCopy()
    {
        if (AvailableCopies >= TotalCopies)
            throw new InvalidOperationException("All copies already accounted for.");
        AvailableCopies++;
    }
}
