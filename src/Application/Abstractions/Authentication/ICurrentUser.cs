namespace Application.Abstractions.Authentication;

public interface ICurrentUser
{
    bool IsAuthenticated { get; }
    string IdentityId { get; }
    string Email { get; }
    string FullName { get; }
    bool IsInRole(string role);
}
