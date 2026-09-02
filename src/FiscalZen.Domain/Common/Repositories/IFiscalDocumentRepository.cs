using FiscalZen.Domain.FiscalDocuments.Entities;
using FiscalZen.Domain.FiscalDocuments.ValueObjects;

namespace FiscalZen.Domain.Common.Repositories;

public interface IFiscalDocumentRepository : IRepository<FiscalDocument>
{
    Task<FiscalDocument?> GetByIdAsync(Guid id, Guid userId, CancellationToken cancellationToken = default);

    Task<FiscalDocument?> GetByAccessKeyAsync(Guid userId, AccessKey accessKey, CancellationToken cancellationToken = default);

    Task<bool> ExistsByAccessKeyAsync(Guid userId, AccessKey accessKey, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<FiscalDocument>> ListAsync(Guid userId, int page, int pageSize, CancellationToken cancellationToken = default);

    Task<int> CountAsync(Guid userId, CancellationToken cancellationToken = default);

}