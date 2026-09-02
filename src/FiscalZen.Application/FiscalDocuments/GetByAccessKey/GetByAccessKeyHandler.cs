using FiscalZen.Application.FiscalDocuments.Common;
using FiscalZen.Application.FiscalDocuments.GetById;
using FiscalZen.Domain.Common.Repositories;

namespace FiscalZen.Application.FiscalDocuments.GetByAccessKey;

public sealed class GetByAccessKeyHandler
{
    private readonly IFiscalDocumentRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public GetByAccessKeyHandler(IFiscalDocumentRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }


    public async Task<FiscalDocumentResponse?> HandleAsync(GetByAccessKeyQuery command, CancellationToken cancellationToken)
    {
        var document = await _repository.GetByAccessKeyAsync(command.UserId, command.AccessKey, cancellationToken);

        if (document is null)
            return null;

        return new FiscalDocumentResponse(
            document.Id,
            document.AccessKey.Value,
            document.Number,
            document.Series,
            document.IssueDate,
            document.ProductsAmount.Value,
            document.FreightAmount.Value,
            document.DiscountAmount.Value,
            document.TotalAmount.Value,
            document.Items);
    }

}
