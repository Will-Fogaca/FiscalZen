using FiscalZen.Domain.FiscalDocuments.Entities;
using Microsoft.EntityFrameworkCore;

namespace FiscalZen.Infrastructure.Persistence;

public sealed class FiscalZenDbContext : DbContext
{
    public DbSet<FiscalDocument> FiscalDocuments => Set<FiscalDocument>();

    public FiscalZenDbContext(DbContextOptions<FiscalZenDbContext> options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FiscalZenDbContext).Assembly);
    }
}