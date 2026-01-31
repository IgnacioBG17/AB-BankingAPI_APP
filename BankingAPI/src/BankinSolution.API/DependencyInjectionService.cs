using Asp.Versioning;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace BankinSolution.API
{
    public static class DependencyInjectionService
    {
        public static IServiceCollection AddWebApi(this IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
                options.ApiVersionReader = ApiVersionReader.Combine(
                    new UrlSegmentApiVersionReader(),
                    new QueryStringApiVersionReader("api-version"),
                    new HeaderApiVersionReader("x-api-version")
                );
            })
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Ingrese un token JWT: Bearer {token}",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                        },
                        Array.Empty<string>()
                    }
                });

                options.DocInclusionPredicate((docName, apiDesc) =>
                {
                    if (!string.IsNullOrEmpty(apiDesc.GroupName))
                        return apiDesc.GroupName == docName;

                    return apiDesc.RelativePath?.IndexOf($"/{docName}/", StringComparison.OrdinalIgnoreCase) >= 0;
                });

                var xmlFile = $"{Assembly.GetEntryAssembly()?.GetName().Name}.xml";
                var xmlPath = System.IO.Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (System.IO.File.Exists(xmlPath))
                {
                    options.IncludeXmlComments(xmlPath);
                }

                options.OperationFilter<RemoveVersionFromParameterOperationFilter>();
                options.DocumentFilter<ReplaceVersionWithExactValueInPath>();
            });

            services.ConfigureOptions<ConfigureSwaggerOptions>();
            return services;
        }

        public class RemoveVersionFromParameterOperationFilter : Swashbuckle.AspNetCore.SwaggerGen.IOperationFilter
        {
            public void Apply(OpenApiOperation operation, OperationFilterContext context)
            {
                if (operation.Parameters == null) return;

                var toRemove = operation.Parameters
                    .Where(p => p.Name.Equals("version", StringComparison.OrdinalIgnoreCase)
                             || p.Name.Equals("api-version", StringComparison.OrdinalIgnoreCase))
                    .ToList();

                foreach (var p in toRemove) operation.Parameters.Remove(p);
            }
        }

        public class ReplaceVersionWithExactValueInPath : Swashbuckle.AspNetCore.SwaggerGen.IDocumentFilter
        {
            public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
            {
                var newPaths = new OpenApiPaths();

                foreach (var path in swaggerDoc.Paths)
                {
                    var newKey = path.Key.Replace("v{version}", swaggerDoc.Info.Version);
                    newPaths.Add(newKey, path.Value);
                }

                swaggerDoc.Paths = newPaths;
            }
        }
    }
}
