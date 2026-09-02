using FiscalZen.Application.Authentication;
using FiscalZen.Application.FiscalDocuments.Delete;
using FiscalZen.Application.FiscalDocuments.GetByAccessKey;
using FiscalZen.Application.FiscalDocuments.GetById;
using FiscalZen.Application.FiscalDocuments.ImportFiscalDocuments;
using FiscalZen.Application.FiscalDocuments.List;
using FiscalZen.Domain.FiscalDocuments.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace FiscalZen.Api.Endpoints.FiscalDocuments;

public static class FiscalDocumentsEndpoints
{
    public static void MapFiscalDocuments(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/fiscal").RequireAuthorization().WithTags("Fiscal Documents");

        group.MapPost("/import", async (HttpRequest request, [FromServices] ImportHandler handler, [FromServices] ICurrentUser currentUser, CancellationToken cancellationToken) =>
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

            var command = new ImportCommand(currentUser.UserId, xmls);

            var response = await handler.HandleAsync(command, cancellationToken);

            return Results.Ok(response);
        });

        group.MapGet("/{id:guid}", async ([FromRoute] Guid id, [FromServices] GetByIdHandler handler, [FromServices] ICurrentUser currentUser, CancellationToken cancellationToken) =>
        {
            var command = new GetByIdQuery(id, currentUser.UserId);

            var response = await handler.HandleAsync(command, cancellationToken);

            if (response is null)
                return Results.NotFound("Nota fiscal não encontrada.");

            return Results.Ok(response);
        });

        group.MapGet("/access-key/{accessKey}", async ([FromRoute] string accessKey, [FromServices] GetByAccessKeyHandler handler, [FromServices] ICurrentUser currentUser, CancellationToken cancellationToken) =>
        {
            var command = new GetByAccessKeyQuery(new AccessKey(accessKey), currentUser.UserId);

            var response = await handler.HandleAsync(command, cancellationToken);

            return response is null ? Results.NotFound() : Results.Ok(response);
        });

        group.MapDelete("/{id:guid}", async ([FromRoute] Guid id, [FromServices] DeleteHandler handler, [FromServices] ICurrentUser currentUser, CancellationToken cancellationToken) =>
        {
            var command = new DeleteCommand(id, currentUser.UserId);

            var deleted = await handler.HandleAsync(command, cancellationToken);

            if (!deleted)
                return Results.NotFound("Nota fiscal não encontrada.");

            return Results.NoContent();
        });

        group.MapGet("", async ([FromQuery] int page, [FromQuery] int pageSize, [FromServices] ListHandler handler, [FromServices] ICurrentUser currentUser, CancellationToken cancellationToken) =>
        {
            var query = new ListQuery(currentUser.UserId, page, pageSize);

            var response = await handler.HandleAsync(query, cancellationToken);

            return Results.Ok(response);
        });
    }
}