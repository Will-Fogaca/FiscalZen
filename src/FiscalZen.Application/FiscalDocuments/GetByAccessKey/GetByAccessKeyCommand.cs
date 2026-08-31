using FiscalZen.Domain.FiscalDocuments.ValueObjects;

namespace FiscalZen.Application.FiscalDocuments.GetByAccessKey;

public sealed record GetByAccessKeyCommand
(
    AccessKey AccessKey,
    Guid AccountId
);