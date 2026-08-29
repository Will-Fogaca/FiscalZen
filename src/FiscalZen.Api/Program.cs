using FiscalZen.Api.Endpoints.FiscalDocuments;
using FiscalZen.Application.FiscalDocuments.Import;
using FiscalZen.Application.FiscalDocuments.ImportFiscalDocuments;
using FiscalZen.Domain.Common.Repositories;
using FiscalZen.Infrastructure.Persistence;
using FiscalZen.Infrastructure.Persistence.Repositories;
using FiscalZen.Infrastructure.Xml;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<FiscalZenDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddScoped<IFiscalDocumentRepository, FiscalDocumentRepository>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IXmlFiscalDocumentParser, NFeXmlParser>();

builder.Services.AddScoped<ImportFiscalDocumentsHandler>();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapImportFiscalDocuments();

app.Run();