using System.Reflection;
using Application;
using Application.Exceptions;
using Elastic.Clients.Elasticsearch;
using Infrastructure;
using Infrastructure.Options;
using Infrastructure.Services.Elastic;
using PublicAPI.Services;
using PublicAPI.Services.Endpoint;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAuthorization();
builder.Services.AddAuthenticationJwtBearer(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddServiceWrapper();
builder.Services.AddConfigurationSerilog(Assembly.GetExecutingAssembly(), builder.Configuration);
builder.Services.AddConfigurationSerilog(Assembly.GetExecutingAssembly(),builder.Configuration);
builder.Host.UseSerilog();

const string corsName = "AllowClientsApp";

builder.Services.AddCors(options =>
{
    var clientAppSection = builder.Configuration.GetSection("ClientConfig").Get<ClientAppOptions>();
    var allowedOrigins = clientAppSection?.Clients
        .Select(x=>x.Address)
        .Where(x=>!string.IsNullOrWhiteSpace(x))
        .Distinct()
        .ToArray();
    options.AddPolicy(corsName,
        policy => policy
            .WithOrigins(allowedOrigins ?? []) 
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()); 
});

var app = builder.Build();

using var scope = app.Services.CreateScope();
var elasticClient = scope.ServiceProvider.GetService<ElasticsearchClient>()
    ?? throw new Exception("Not found Elasticsearch client to mapping index");
var seeder = new SeederIndex(elasticClient);
await seeder.IndexMappingAsync();

// Configure the HTTP request pipeline.
app.MapEndpoints();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddlewareHandling>();

app.UseHttpsRedirection();

app.UseCors(corsName);

app.UseAuthentication();

app.UseAuthorization();

app.Run();

