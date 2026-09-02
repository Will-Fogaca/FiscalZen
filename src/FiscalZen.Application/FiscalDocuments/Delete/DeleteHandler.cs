using FiscalZen.Domain.Common.Repositories;

namespace FiscalZen.Application.FiscalDocuments.Delete;

public sealed class DeleteHandler
{
    private readonly IFiscalDocumentRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteHandler(IFiscalDocumentRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> HandleAsync(DeleteCommand command, CancellationToken cancellationToken = default)
    {
        var document = await _repository.GetByIdAsync(
            command.Id,
            command.UserId,
            cancellationToken);

        if (document is null)
            return false;

        _repository.Remove(document);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}