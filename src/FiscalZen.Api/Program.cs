using FiscalZen.Api.Endpoints.FiscalDocuments;
using FiscalZen.Application.Authentication;
using FiscalZen.Application.FiscalDocuments.GetByAccessKey;
using FiscalZen.Application.FiscalDocuments.GetById;
using FiscalZen.Application.FiscalDocuments.Import;
using FiscalZen.Application.FiscalDocuments.ImportFiscalDocuments;
using FiscalZen.Domain.Common.Repositories;
using FiscalZen.Infrastructure.Identity;
using FiscalZen.Infrastructure.Persistence;
using FiscalZen.Infrastructure.Persistence.Repositories;
using FiscalZen.Infrastructure.Xml;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<FiscalZenDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddDbContext<IdentityDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

builder.Services
    .AddIdentityApiEndpoints<ApplicationUser>()
    .AddRoles<IdentityRole<Guid>>()
    .AddEntityFrameworkStores<IdentityDbContext>();

builder.Services.Configure<AuthenticationOptions>(options =>
{
    options.DefaultScheme =
        IdentityConstants.ApplicationScheme;

    options.DefaultAuthenticateScheme =
        IdentityConstants.ApplicationScheme;

    options.DefaultChallengeScheme =
        IdentityConstants.ApplicationScheme;

    options.DefaultSignInScheme =
        IdentityConstants.ApplicationScheme;
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Events.OnRedirectToLogin = context =>
    {
        context.Response.StatusCode =
            StatusCodes.Status401Unauthorized;

        return Task.CompletedTask;
    };

    options.Events.OnRedirectToAccessDenied = context =>
    {
        context.Response.StatusCode =
            StatusCodes.Status403Forbidden;

        return Task.CompletedTask;
    };
});

builder.Services.AddScoped(
    typeof(IRepository<>),
    typeof(Repository<>));

builder.Services.AddScoped<
    IFiscalDocumentRepository,
    FiscalDocumentRepository>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<
    IXmlFiscalDocumentParser,
    NFeXmlParser>();

builder.Services.AddScoped<ImportHandler>();
builder.Services.AddScoped<GetByIdHandler>();
builder.Services.AddScoped<GetByAccessKeyHandler>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<
    ICurrentAccount,
    CurrentAccount>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapGroup("/api/auth")
    .MapIdentityApi<ApplicationUser>();

app.MapFiscalDocuments();

app.Run();