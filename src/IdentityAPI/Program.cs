using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using IdentityAPI.Data;
using IdentityAPI.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Abstractions;
using OpenIddict.EntityFrameworkCore.Models;
using Serilog;
using Shared;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthorization();
builder.Services.AddSwaggerGen();
builder.Services.AddConfigurationSerilog(Assembly.GetExecutingAssembly(),builder.Configuration);
builder.Host.UseSerilog();  
builder.Services.AddDbContext<ApplicationIdentityDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
    options.UseOpenIddict();
});
builder.Services.AddIdentity<ApplicationIdentityUser, ApplicationIdentityRole>()
    .AddEntityFrameworkStores<ApplicationIdentityDbContext>()
    .AddDefaultTokenProviders();
builder.Services.AddOpenIddict()
    .AddCore(options =>
    {
        options.UseEntityFrameworkCore()
            .UseDbContext<ApplicationIdentityDbContext>();
    })
    .AddServer(options =>
    {
        options.SetAuthorizationEndpointUris("/connect/authorize");
        options.SetTokenEndpointUris("/connect/token");

        options.AllowAuthorizationCodeFlow();
        options.AllowPasswordFlow();
        options.AllowRefreshTokenFlow();
        
        options.RegisterScopes("api","offline_access");
        options.UseAspNetCore()
            .EnableAuthorizationEndpointPassthrough()
            .EnableTokenEndpointPassthrough();
        options.DisableAccessTokenEncryption();
        
        options.SetAccessTokenLifetime(TimeSpan.FromMinutes(15));
        options.SetRefreshTokenLifetime(TimeSpan.FromDays(7));
       
        options.AddSigningCertificate(X509CertificateLoader.LoadPkcs12FromFile("C:\\Windows\\System32\\certificate.pfx","211885"));
        options.AddEncryptionCertificate(X509CertificateLoader.LoadPkcs12FromFile("C:\\Windows\\System32\\certificate.pfx","211885"));
    });
var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapEndpoints();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using var scope = app.Services.CreateScope();
var applicationManager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();
var countApplication = await applicationManager.CountAsync();
if (countApplication <= 0)
{
    await applicationManager.CreateAsync(new OpenIddictApplicationDescriptor()
    {
        ClientId = "web-app",
        ClientSecret = "my-secret",
        Permissions =
        {
            OpenIddictConstants.Permissions.Endpoints.Token,
            OpenIddictConstants.Permissions.GrantTypes.Password,
            OpenIddictConstants.Permissions.GrantTypes.RefreshToken,
            OpenIddictConstants.Permissions.Scopes.Profile,
            OpenIddictConstants.Permissions.Scopes.Email,
        }
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.Run();
