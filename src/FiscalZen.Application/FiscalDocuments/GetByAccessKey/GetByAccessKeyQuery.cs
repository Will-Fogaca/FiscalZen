using FiscalZen.Domain.FiscalDocuments.ValueObjects;

namespace FiscalZen.Application.FiscalDocuments.GetByAccessKey;

public sealed record GetByAccessKeyQuery
(
    AccessKey AccessKey,
    Guid UserId
);