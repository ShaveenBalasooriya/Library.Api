using System.Net.Mail;
using Domain.Enums;
using Domain.Primitives;
using Domain.Shared;

namespace Domain.ValueObjects;

public sealed class Email : ValueObject
{
    public string Value { get; init; }
    private Email(string value) => Value = value;

    public static Result<Email> Create(string emailValue)
    {
        if (string.IsNullOrWhiteSpace(emailValue))
        {
            return Result<Email>.Failure(new Error("Email.Empty", "Email address cannot be empty.", ErrorType.Validation));
        }

        if (!MailAddress.TryCreate(emailValue, out var mailAddress) || mailAddress.Address != emailValue)
        {
            return Result<Email>.Failure(new Error("Email.InvalidFormat", "Email address format is invalid.", ErrorType.Validation));
        }

        return Result<Email>.Success(new Email(emailValue));
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
