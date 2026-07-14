using System.ComponentModel.DataAnnotations;

namespace Library.Api.Contracts.Members;

public record class UpdateMemberRequest(
    [Required] string FullName,
    string PhoneNumber,
    bool IsActive
);
