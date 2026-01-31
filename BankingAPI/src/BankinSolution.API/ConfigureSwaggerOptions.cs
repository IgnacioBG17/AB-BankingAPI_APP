using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace BankinSolution.API
{
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider;

        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
        {
            _provider = provider;
        }

        public void Configure(SwaggerGenOptions options)
        {
            foreach (var description in _provider.ApiVersionDescriptions)
            {
                var info = new OpenApiInfo
                {
                    Title = "Sistematica Banking API - Technical Test",
                    Version = description.GroupName,
                    Description = description.IsDeprecated
                        ? "API (obsoleta) - esta versión está marcada como deprecated."
                        : "Administración de APIs para Banking API TechnicalTest App"
                };

                options.SwaggerDoc(description.GroupName, info);
            }
        }
    }
}
