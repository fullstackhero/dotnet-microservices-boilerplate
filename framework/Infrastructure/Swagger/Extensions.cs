using FSH.Framework.Infrastructure.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace FSH.Framework.Infrastructure.Swagger
{
    internal static class Extensions
    {
        public static void UseSwaggerExtension(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (!env.IsProduction())
            {
                app.UseSwagger(c =>
                {
                    c.RouteTemplate = "docs/{documentName}/openapi.json";
                    c.PreSerializeFilters.Add((swagger, httpReq) => swagger.Servers = new List<OpenApiServer> { new OpenApiServer { Url = $"{httpReq.Scheme}://{httpReq.Host.Value}{httpReq.PathBase.Value}" } });
                });
                app.UseSwaggerUI(config =>
                {
                    config.SwaggerEndpoint("v1/openapi.json", "Version 1");
                    config.RoutePrefix = "docs";
                    config.DocExpansion(DocExpansion.List);
                    config.DisplayRequestDuration();
                    config.DefaultModelsExpandDepth(-1);
                });
            }
        }
        internal static void AddSwaggerExtension(this IServiceCollection services, IConfiguration configuration)
        {
            var swaggerOptions = services.BindValidateReturn<SwaggerOptions>(configuration);
            _ = services.AddSwaggerGen(config =>
            {
                config.CustomSchemaIds(type => type.ToString());
                config.MapType<DateOnly>(() => new OpenApiSchema
                {
                    Type = "string",
                    Format = "date"
                });

                config.SwaggerDoc(
                    "v1",
                    new OpenApiInfo
                    {
                        Version = "v1",
                        Title = swaggerOptions.Title,
                        Description = swaggerOptions.Description,
                        Contact = new OpenApiContact
                        {
                            Name = swaggerOptions.Name,
                            Email = swaggerOptions.Email,
                        },
                    });

                config.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                    new OpenApiSecurityScheme {
                        Reference = new OpenApiReference {
                            Type = ReferenceType.SecurityScheme,
                                Id = JwtBearerDefaults.AuthenticationScheme
                        }
                    },
                    Array.Empty<string>()
                }});

                config.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description = "Input your Bearer token to access this API",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                    BearerFormat = "JWT",
                });
            });
        }
    }
}
