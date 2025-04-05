using System.Reflection;
using Application;
using Infrastructure;
using PublicAPI.Services;
using Serilog;
using Shared;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAuthorization();
builder.Services.AddAuthenticationJwtBearer();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
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

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.Run();

