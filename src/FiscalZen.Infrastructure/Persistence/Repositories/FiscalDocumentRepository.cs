using FiscalZen.Domain.Common.Repositories;
using FiscalZen.Domain.FiscalDocuments.Entities;
using FiscalZen.Domain.FiscalDocuments.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace FiscalZen.Infrastructure.Persistence.Repositories;

public sealed class FiscalDocumentRepository : Repository<FiscalDocument>, IFiscalDocumentRepository
{
    public FiscalDocumentRepository(FiscalZenDbContext context) : base(context) { }

    public async Task<FiscalDocument?> GetByIdAsync(Guid id, Guid userId, CancellationToken cancellationToken = default)
    {
        return await DbSet.AsNoTracking().Include(x => x.Items).FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId, cancellationToken);
    }

    public async Task<FiscalDocument?> GetByAccessKeyAsync(Guid userId, AccessKey accessKey, CancellationToken cancellationToken = default)
    {
        return await DbSet.AsNoTracking().Include(x => x.Items).FirstOrDefaultAsync(x => x.UserId == userId && x.AccessKey == accessKey, cancellationToken);
    }

    public async Task<bool> ExistsByAccessKeyAsync(Guid userId, AccessKey accessKey, CancellationToken cancellationToken = default)
    {
        return await DbSet.AnyAsync(x => x.UserId == userId && x.AccessKey == accessKey, cancellationToken);
    }

    public async Task<IReadOnlyCollection<FiscalDocument>> ListAsync(Guid userId, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.IssueDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> CountAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await DbSet.CountAsync(x => x.UserId == userId, cancellationToken);
    }
}