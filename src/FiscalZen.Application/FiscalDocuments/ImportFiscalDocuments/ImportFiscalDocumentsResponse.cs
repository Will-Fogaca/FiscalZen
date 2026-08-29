namespace FiscalZen.Application.FiscalDocuments.ImportFiscalDocuments;

public sealed record ImportFiscalDocumentsResponse(int ImportedCount, int IgnoredCount, IReadOnlyCollection<Guid> DocumentIds);