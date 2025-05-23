using System.Reflection;
using Application;
using Application.Exceptions;
using Application.Helper;
using Infrastructure;
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

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapEndpoints();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddlewareHandling>();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.Run();

