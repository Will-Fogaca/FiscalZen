using FiscalZen.Domain.Common.Repositories;
using FiscalZen.Domain.FiscalDocuments.Entities;
using FiscalZen.Domain.FiscalDocuments.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace FiscalZen.Infrastructure.Persistence.Repositories;

public sealed class FiscalDocumentRepository : Repository<FiscalDocument>, IFiscalDocumentRepository
{
    public FiscalDocumentRepository(FiscalZenDbContext context) : base(context) {}

    public async Task<FiscalDocument?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await DbSet.AsNoTracking().Include(x => x.Items).FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<FiscalDocument?> GetByAccessKeyAsync(Guid accountId, AccessKey accessKey, CancellationToken cancellationToken = default)
    {
        return await DbSet.AsNoTracking().Include(x => x.Items).FirstOrDefaultAsync(x => x.AccountId == accountId && x.AccessKey == accessKey, cancellationToken);
    }

    public async Task<bool> ExistsByAccessKeyAsync(Guid accountId, AccessKey accessKey, CancellationToken cancellationToken = default)
    {
        return await DbSet.AnyAsync(x => x.AccountId == accountId && x.AccessKey == accessKey, cancellationToken);
    }
}