using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace PublicAPI.Services;

public static class AuthenticationExtension
{
    public static IServiceCollection AddAuthenticationJwtBearer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc(configuration["ApiSwaggerDocs:Version"], new OpenApiInfo()
            {
                Title = configuration["ApiSwaggerDocs:Title"],
                Version = configuration["ApiSwaggerDocs:Version"],
            });
            var securitySchema = new OpenApiSecurityScheme()
            {
                Name = configuration["ApiSwaggerDocs:OpenApiSecurityScheme:Name"],
                Description = configuration["ApiSwaggerDocs:OpenApiSecurityScheme:Description"],
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme =  configuration["ApiSwaggerDocs:OpenApiSecurityScheme:Schema"],
                BearerFormat = configuration["ApiSwaggerDocs:OpenApiSecurityScheme:BearerFormat"],
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
                options.Audience = configuration["JWTConfiguration:Audience"];
                options.RequireHttpsMetadata = false;
                options.MetadataAddress = configuration["KeyCloakAuthentication:MetadataAddress"]
                                          ?? throw new Exception("KeyCloakAuthentication:MetadataAddress not set in configuration");
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["KeyCloakAuthentication:ValidIssuer"],
                };
            });
        return services;
    }
}
