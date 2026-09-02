namespace FiscalZen.Application.Authentication;

public interface ICurrentUser
{
    Guid UserId { get; }
}
