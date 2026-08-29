using FiscalZen.Domain.FiscalDocuments.Entities;
using FiscalZen.Domain.FiscalDocuments.ValueObjects;

namespace FiscalZen.Domain.Common.Repositories;

public interface IFiscalDocumentRepository : IRepository<FiscalDocument>
{
    Task<FiscalDocument?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<FiscalDocument?> GetByAccessKeyAsync(Guid accountId, AccessKey accessKey, CancellationToken cancellationToken = default);

    Task<bool> ExistsByAccessKeyAsync(Guid accountId, AccessKey accessKey, CancellationToken cancellationToken = default);
}