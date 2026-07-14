using System.ComponentModel.DataAnnotations;

namespace Library.Api.Contracts.Members;

public record class RegisterMemberRequest(
    [Required] string FullName,
    [Required][EmailAddress] string Email,
    string PhoneNumber
);
