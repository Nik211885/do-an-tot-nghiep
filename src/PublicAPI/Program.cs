using System.Reflection;
using Application;
using Application.Exceptions;
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
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        policy => policy
            .WithOrigins(builder.Configuration["FontEnd"]?? string.Empty) 
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()); 
});

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

app.UseCors("AllowAngularApp");

app.UseAuthentication();

app.UseAuthorization();

app.Run();

