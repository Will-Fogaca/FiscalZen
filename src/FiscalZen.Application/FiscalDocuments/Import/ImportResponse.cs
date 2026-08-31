namespace FiscalZen.Application.FiscalDocuments.ImportFiscalDocuments;

public sealed record ImportResponse(int ImportedCount, int IgnoredCount, IReadOnlyCollection<Guid> DocumentIds);