using FiscalZen.Domain.Common.Repositories;

namespace FiscalZen.Application.FiscalDocuments.List;

public sealed class ListHandler
{
    private readonly IFiscalDocumentRepository _repository;

    public ListHandler(IFiscalDocumentRepository repository)
    {
        _repository = repository;
    }

    public async Task<ListResponse> HandleAsync(
        ListQuery query,
        CancellationToken cancellationToken = default)
    {
        var documents = await _repository.ListAsync(
            query.UserId,
            query.Page,
            query.PageSize,
            cancellationToken);

        var totalItems = await _repository.CountAsync(
            query.UserId,
            cancellationToken);

        var items = documents
            .Select(document => new FiscalDocumentListItem(
                document.Id,
                document.AccessKey.Value,
                document.Number,
                document.Series,
                document.IssueDate,
                document.TotalAmount.Value))
            .ToList();

        var totalPages = (int)Math.Ceiling(
            totalItems / (double)query.PageSize);

        return new ListResponse(
            items,
            query.Page,
            query.PageSize,
            totalItems,
            totalPages);
    }
}