using Domain.Primitives;

namespace Domain.Entities
{
    public sealed class Book : Entity
    {
        public Book ( Guid id, string title, string author, string isbn, int publishedYear, int totalCopies, int availableCopies) : base(id)
        {
            Title = title;
            Author = author;
            Isbn = isbn;
            PublishedYear = publishedYear;
            TotalCopies = totalCopies;
            AvailableCopies = availableCopies;
        }
        public string Title { get; }
        public string Author { get; }
        public string Isbn { get; }
        public int PublishedYear { get; }
        public int TotalCopies { get; }
        public int AvailableCopies { get; }
    }
}
