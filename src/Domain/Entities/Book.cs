using Domain.Enums;
using Domain.Primitives;
using Domain.Shared;
using Domain.ValueObjects;

namespace Domain.Entities
{
    public sealed class Book : Entity
    {
        public string Title { get; private set; }
        public string Author { get; private set; }
        public Isbn Isbn { get; private set; }
        public PublishedYear PublishedYear { get; private set; }
        public BookCopies Copies { get; private set; }

        private Book(Guid id, string title, string author, Isbn isbn, PublishedYear publishedYear, BookCopies copies) : base(id)
        {
            Title = title;
            Author = author;
            Isbn = isbn;
            PublishedYear = publishedYear;
            Copies = copies;
        }

        public static Result<Book> Create(string title, string author, Isbn isbn, PublishedYear publishedYear, BookCopies copies)
        {
            if (string.IsNullOrWhiteSpace(title)) return Result<Book>.Failure(new Error("Book.TitleEmpty", "Title is empty.", ErrorType.Validation));
            if (string.IsNullOrWhiteSpace(author)) return Result<Book>.Failure(new Error("Book.AuthorEmpty", "Author is empty.", ErrorType.Validation));

            return Result<Book>.Success(new Book(Guid.CreateVersion7(), title, author, isbn, publishedYear, copies));
        }

        public Result Update(string title, string author, Isbn isbn, PublishedYear publishedYear, int totalCopies)
        {
            if (string.IsNullOrWhiteSpace(title)) return Result.Failure(new Error("Book.TitleEmpty", "Title is empty.", ErrorType.Validation));
            if (string.IsNullOrWhiteSpace(author)) return Result.Failure(new Error("Book.AuthorEmpty", "Author is empty.", ErrorType.Validation));

            var updatedCopiesResult = Copies.UpdateTotal(totalCopies);

            if (updatedCopiesResult.IsFailure)
            {
                return Result.Failure(updatedCopiesResult.Error);
            }

            Title = title;
            Author = author;
            Isbn = isbn;
            PublishedYear = publishedYear;
            Copies = updatedCopiesResult.Value;

            return Result.Success();
        }

        public Result BorrowCopy()
        {
            var borrowResult = Copies.Borrow();

            if (borrowResult.IsFailure)
            {
                return Result.Failure(borrowResult.Error);
            }

            Copies = borrowResult.Value;

            return Result.Success();
        }

        public Result ReturnCopy()
        {
            var returnResult = Copies.Return();

            if (returnResult.IsFailure)
            {
                return Result.Failure(returnResult.Error);
            }

            Copies = returnResult.Value;

            return Result.Success();
        }
    }
}