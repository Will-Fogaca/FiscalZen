using FiscalZen.Api.Endpoints.FiscalDocuments;
using FiscalZen.Application.Authentication;
using FiscalZen.Application.FiscalDocuments.Delete;
using FiscalZen.Application.FiscalDocuments.GetByAccessKey;
using FiscalZen.Application.FiscalDocuments.GetById;
using FiscalZen.Application.FiscalDocuments.Import;
using FiscalZen.Application.FiscalDocuments.ImportFiscalDocuments;
using FiscalZen.Application.FiscalDocuments.List;
using FiscalZen.Domain.Common.Repositories;
using FiscalZen.Infrastructure.Identity;
using FiscalZen.Infrastructure.Persistence;
using FiscalZen.Infrastructure.Persistence.Repositories;
using FiscalZen.Infrastructure.Xml;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<FiscalZenDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddDbContext<IdentityDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddAuthorization();

builder.Services.AddIdentityApiEndpoints<ApplicationUser>().AddRoles<IdentityRole<Guid>>().AddEntityFrameworkStores<IdentityDbContext>();

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddScoped<IFiscalDocumentRepository, FiscalDocumentRepository>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IXmlFiscalDocumentParser, NFeXmlParser>();

builder.Services.AddScoped<ImportHandler>();
builder.Services.AddScoped<GetByIdHandler>();
builder.Services.AddScoped<GetByAccessKeyHandler>();
builder.Services.AddScoped<DeleteHandler>();
builder.Services.AddScoped<ListHandler>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<ICurrentUser, CurrentUser>();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition(
        "Bearer",
        new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "Bearer",
            In = ParameterLocation.Header,
            Description = "Informe o access token retornado pelo login."
        });

    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("Bearer", document)] = []
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapGroup("/api/auth").MapIdentityApi<ApplicationUser>().WithTags("Authentication");

app.MapFiscalDocuments();

app.Run();