using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using ShortUrl.Api.Configuration;
using ShortUrl.Api.Validators;
using ShortUrl.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddShortUrl(builder.Configuration);
builder.Services.AddValidatorsFromAssemblyContaining<CreateShortLinkDtoValidator>();
builder.Services.AddControllers();

builder.Services.AddApiVersioning(options => {
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
    options.ReportApiVersions = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.UseApiBehavior = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
});
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddVersionedApiExplorer(options => {
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

builder.Services.AddSwaggerGen();
builder.Services.ConfigureOptions<SwaggerOptionsConfigurator>();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddResponseCaching();

var app = builder.Build();
app.UseResponseCaching();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options => {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "V1");
    });
}

app.MapControllers();
app.Run();

public partial class Program {}
