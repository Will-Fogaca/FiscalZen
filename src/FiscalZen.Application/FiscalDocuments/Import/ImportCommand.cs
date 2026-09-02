namespace FiscalZen.Application.FiscalDocuments.ImportFiscalDocuments;

public sealed record ImportCommand(Guid UserId, IEnumerable<string> Xmls);