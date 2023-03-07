using FluentAssertions.Common;
using FluentValidation;
using ShortUrl.Api.Configuration;
using ShortUrl.Api.Options;
using ShortUrl.Api.Validators;
using ShortUrl.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddShortUrl(builder.Configuration);
builder.Services.AddValidatorsFromAssemblyContaining<CreateShortLinkDtoValidator>();
builder.Services.AddControllers();
builder.Services.ConfigureSwagger();


builder.Services.AddOptions<CacheOptions>()
    .Bind(builder.Configuration.GetSection(CacheOptions.SectionName))
    .Validate(options => options is { MaxSize: >= 0, CacheTokensTime: >= 0, CacheImagesTime: >= 0 })
    .ValidateOnStart();


builder.Services.AddDistributedMemoryCache(options => {
    var cacheOptions = builder.Configuration
        .GetSection(CacheOptions.SectionName)
        .Get<CacheOptions>() ?? new CacheOptions();
    options.SizeLimit = 1024 * 1024 * cacheOptions.MaxSize;
});

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
