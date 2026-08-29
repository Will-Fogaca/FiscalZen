using FiscalZen.Domain.Common.Repositories;

namespace FiscalZen.Infrastructure.Persistence;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly FiscalZenDbContext _context;

    public UnitOfWork(FiscalZenDbContext context)
    {
        _context = context;
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }
}