namespace FiscalZen.Application.FiscalDocuments.Delete;

public sealed record DeleteCommand(
    Guid Id,
    Guid UserId
);