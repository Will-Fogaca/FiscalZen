using FiscalZen.Application.FiscalDocuments.GetByAccessKey;
using FiscalZen.Application.FiscalDocuments.GetById;
using FiscalZen.Application.FiscalDocuments.ImportFiscalDocuments;
using FiscalZen.Domain.Common.Exceptions;
using FiscalZen.Domain.FiscalDocuments.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace FiscalZen.Api.Endpoints.FiscalDocuments;

public static class FiscalDocumentsEndpoints
{
    public static void MapFiscalDocuments(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/fiscal");

        group.MapPost("/import", async (HttpRequest request, [FromServices] ImportHandler handler, CancellationToken cancellationToken) =>
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

            var command = new ImportCommand(accountId, xmls);

            var response = await handler.HandleAsync(command, cancellationToken);

            return Results.Ok(response);
        });


        group.MapGet("/{id:guid}", async ([FromRoute] Guid id, [FromServices] GetByIdHandler handler, CancellationToken cancellationToken) =>
        {
            var accountId = Guid.Parse("11111111-1111-1111-1111-111111111111");

            var command = new GetByIdCommand(id, accountId);

            var response = await handler.HandleAsync(command, cancellationToken);
             
            if (response is null)
                return Results.NotFound("Nota fiscal não encontrada.");

            return Results.Ok(response);
        });

        group.MapGet("/access-key/{accessKey}", async ([FromRoute] string accessKey, [FromServices] GetByAccessKeyHandler handler, CancellationToken cancellationToken) =>
        {
            var accountId = Guid.Parse("11111111-1111-1111-1111-111111111111");

            var command = new GetByAccessKeyCommand(
                new AccessKey(accessKey),
                accountId);

            var response = await handler.HandleAsync(command, cancellationToken);

            return response is null? Results.NotFound(): Results.Ok(response);
        });
    }
}