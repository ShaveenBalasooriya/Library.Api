namespace Domain.Shared;

public class Result<T>: Result
{
    public T Value { get; }

    private Result(bool isSuccess, Error error, T value): base(isSuccess, error)
    {
        Value = value;
    }

    public static Result<T> Success(T value) => new(true, Error.None, value);

    // Remove this and see what happens... fuck around and find out 🙏🏽
    public static new Result<T> Failure(Error error) => new(false, error, default!);
}
