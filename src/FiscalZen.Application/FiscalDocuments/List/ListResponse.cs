namespace FiscalZen.Application.FiscalDocuments.List;

public sealed record ListResponse
(
    IReadOnlyCollection<FiscalDocumentListItem> Items,
    int Page,
    int PageSize,
    int TotalItems,
    int TotalPages
);