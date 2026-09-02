namespace FiscalZen.Application.FiscalDocuments.List;

public sealed record ListQuery
(
    Guid UserId,
    int Page = 1,
    int PageSize = 20    
);
