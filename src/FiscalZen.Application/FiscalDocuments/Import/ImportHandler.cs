using FiscalZen.Application.FiscalDocuments.Import;
using FiscalZen.Domain.Common.Repositories;

namespace FiscalZen.Application.FiscalDocuments.ImportFiscalDocuments;

public sealed class ImportHandler
{
    private readonly IXmlFiscalDocumentParser _parser;
    private readonly IFiscalDocumentRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public ImportHandler(IXmlFiscalDocumentParser parser, IFiscalDocumentRepository repository, IUnitOfWork unitOfWork)
    {
        _parser = parser;
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ImportResponse> HandleAsync(ImportCommand command, CancellationToken cancellationToken = default)
    {
        var documentIds = new List<Guid>();

        var importedCount = 0;
        var ignoredCount = 0;

        foreach (var xml in command.Xmls)
        {
            var document = _parser.Parse(xml);

            document.AssignUser(command.UserId);

            var exists = await _repository.ExistsByAccessKeyAsync(
                command.UserId,
                document.AccessKey,
                cancellationToken);

            if (exists)
            {
                ignoredCount++;
                continue;
            }

            await _repository.AddAsync(document, cancellationToken);

            documentIds.Add(document.Id);
            importedCount++;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new ImportResponse(importedCount, ignoredCount, documentIds);
    }
}