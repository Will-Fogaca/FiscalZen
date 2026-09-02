namespace FiscalZen.Application.FiscalDocuments.List;

public sealed record FiscalDocumentListItem
(
    Guid Id,
    string AccessKey,
    int Number,
    int Series,
    DateTime IssueDate,
    decimal TotalAmount
);