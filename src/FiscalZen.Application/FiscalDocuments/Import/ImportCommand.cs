namespace FiscalZen.Application.FiscalDocuments.ImportFiscalDocuments;

public sealed record ImportCommand(Guid AccountId, IEnumerable<string> Xmls);