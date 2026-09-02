using FiscalZen.Application.FiscalDocuments.Common;
using FiscalZen.Domain.Common.Repositories;

namespace FiscalZen.Application.FiscalDocuments.GetById;

public sealed class GetByIdHandler
{
    private readonly IFiscalDocumentRepository _repository;

    public GetByIdHandler(IFiscalDocumentRepository repository)
    {
        _repository = repository;
    }

    public async Task<FiscalDocumentResponse?> HandleAsync(GetByIdQuery command, CancellationToken cancellationToken = default)
    {
        var document = await _repository.GetByIdAsync(command.Id, command.UserId, cancellationToken);

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