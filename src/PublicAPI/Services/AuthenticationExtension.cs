using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace PublicAPI.Services;

public static class AuthenticationExtension
{
    public static IServiceCollection AddAuthenticationJwtBearer(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo()
            {
                Title = "Business app",
                Version = "v1",
            });
            var securitySchema = new OpenApiSecurityScheme()
            {
                Name = "JWT Authentication",
                Description = "JWT Authentication Authorization header using the Bearer scheme.",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                Reference = new OpenApiReference()
                {
                    Id =  JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme,
                }
            };
            options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, securitySchema);
            options.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {securitySchema, [] }
            });
            options.CustomSchemaIds(scheme => scheme.FullName);
        });
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = "https://localhost:7294";
                options.RequireHttpsMetadata = true;
                options.MetadataAddress = "https://localhost:7294/.well-known/openid-configuration";
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidAudience = "https://localhost:7098",
                };
            });
        return services;
    }
}
