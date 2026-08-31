using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FiscalZen.Infrastructure.Identity;

public sealed class IdentityDbContext: IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
{
    public IdentityDbContext(DbContextOptions<IdentityDbContext> options): base(options)
    {
    }
}