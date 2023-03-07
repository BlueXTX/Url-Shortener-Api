using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;

namespace ShortUrl.Api.Configuration;

public static class SwaggerConfigurator {
    public static void ConfigureSwagger(this IServiceCollection services)
    {
        services.AddApiVersioning(options => {
            options.ApiVersionReader = new UrlSegmentApiVersionReader();
            options.ReportApiVersions = true;
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.UseApiBehavior = true;
            options.AssumeDefaultVersionWhenUnspecified = true;
        });

        services.AddEndpointsApiExplorer();
        services.AddVersionedApiExplorer(options => {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });

        services.AddSwaggerGen();
        services.ConfigureOptions<SwaggerOptionsConfigurator>();
    }
}
