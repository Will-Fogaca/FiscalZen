namespace FiscalZen.Application.FiscalDocuments.ImportFiscalDocuments;

public sealed record ImportFiscalDocumentsCommand(Guid AccountId, IEnumerable<string> Xmls);