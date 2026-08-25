using Domain.Primitives;
using Domain.Shared;

namespace Domain.ValueObjects;

public sealed class BookCopies : ValueObject
{
    public int Total { get; init; }
    public int Available { get; init; }

    public int Borrowed => Total - Available;

    private BookCopies(int total, int available)
    {
        Total = total;
        Available = available;
    }

    public static Result<BookCopies> Create(int total)
    {
        if (total <= 0)
        {
            return Result<BookCopies>.Failure(new Error("BookCopies.TotalInvalid", "Total copies must be greater than zero."));
        }

        return Result<BookCopies>.Success(new BookCopies(total, total));
    }
    public static Result<BookCopies> Create(int total, int available)
    {
        if (total <= 0)
        {
            return Result<BookCopies>.Failure(new Error("BookCopies.TotalInvalid", "Total copies must be greater than zero."));
        }

        if (available < 0 || available > total)
        {
            return Result<BookCopies>.Failure(new Error("BookCopies.AvailableInvalid", "Available copies cannot exceed total copies or be negative."));
        }

        return Result<BookCopies>.Success(new BookCopies(total, available));
    }

    public Result<BookCopies> Borrow()
    {
        if (Available <= 0)
        {
            return Result<BookCopies>.Failure(new Error("BookCopies.NoAvailableCopies", "No copies are currently available to borrow."));
        }

        return Result<BookCopies>.Success(new BookCopies(Total, Available - 1));
    }

    public Result<BookCopies> Return()
    {
        if (Available >= Total)
        {
            return Result<BookCopies>.Failure(new Error("BookCopies.AllCopiesReturned", "All copies are already accounted for in inventory."));
        }

        return Result<BookCopies>.Success(new BookCopies(Total, Available + 1));
    }

    public Result<BookCopies> UpdateTotal(int newTotal)
    {
        if (newTotal <= 0)
        {
            return Result<BookCopies>.Failure(new Error("BookCopies.TotalInvalid", "Total copies cannot be negative."));
        }

        if (newTotal < Borrowed)
        {
            return Result<BookCopies>.Failure(new Error("BookCopies.ReduceBelowBorrowed", "Cannot reduce total copies below currently borrowed count."));
        }
        return Result<BookCopies>.Success(new BookCopies(newTotal, newTotal - Borrowed));
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Total;
        yield return Available;
    }
}
