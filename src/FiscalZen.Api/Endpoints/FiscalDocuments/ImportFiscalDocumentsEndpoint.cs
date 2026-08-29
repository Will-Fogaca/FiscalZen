using FiscalZen.Application.FiscalDocuments.ImportFiscalDocuments;

namespace FiscalZen.Api.Endpoints.FiscalDocuments;

public static class ImportFiscalDocumentsEndpoint
{
    public static void MapImportFiscalDocuments(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/fiscal-documents/import", async (HttpRequest request, ImportFiscalDocumentsHandler handler, CancellationToken cancellationToken) =>
        {
            if (!request.HasFormContentType)
                return Results.BadRequest("A requisição deve utilizar multipart/form-data.");

            var form = await request.ReadFormAsync(cancellationToken);

            var files = form.Files.GetFiles("files");

            if (files.Count == 0)
                return Results.BadRequest("Nenhum arquivo XML foi informado.");

            var xmls = new List<string>();

            foreach (var file in files)
            {
                if (file.Length == 0)
                    continue;

                using var reader = new StreamReader(file.OpenReadStream());

                var xml = await reader.ReadToEndAsync(cancellationToken);

                xmls.Add(xml);
            }

            if (xmls.Count == 0)
                return Results.BadRequest("Nenhum arquivo XML válido foi informado.");

            var accountId = Guid.Parse("11111111-1111-1111-1111-111111111111");

            var command = new ImportFiscalDocumentsCommand(
                accountId,
                xmls);

            var response = await handler.HandleAsync(
                command,
                cancellationToken);

            return Results.Ok(response);
        })
        .WithName("ImportFiscalDocuments")
        .DisableAntiforgery()
        .Produces<ImportFiscalDocumentsResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest);
    }
}