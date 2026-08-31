using FiscalZen.Application.Authentication;
using Microsoft.AspNetCore.Http;

namespace FiscalZen.Infrastructure.Identity;

public sealed class CurrentAccount : ICurrentAccount
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentAccount(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid AccountId
    {
        get
        {
            var accountId = _httpContextAccessor.HttpContext?.User.FindFirst("account_id")?.Value;

            if (!Guid.TryParse(accountId, out var id))
                throw new UnauthorizedAccessException("AccountId não encontrado no usuário autenticado.");

            return id;
        }
    }
}