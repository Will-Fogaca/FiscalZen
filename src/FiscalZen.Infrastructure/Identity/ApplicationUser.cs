using Microsoft.AspNetCore.Identity;

namespace FiscalZen.Infrastructure.Identity;

public sealed class ApplicationUser : IdentityUser<Guid>
{
    public Guid AccountId { get; set; }
}