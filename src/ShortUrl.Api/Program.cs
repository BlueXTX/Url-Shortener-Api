using FluentValidation;
using ShortUrl.Api.Configuration;
using ShortUrl.Api.Validators;
using ShortUrl.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddShortUrl(builder.Configuration);
builder.Services.AddValidatorsFromAssemblyContaining<CreateShortLinkDtoValidator>();
builder.Services.AddControllers();
builder.Services.ConfigureSwagger();

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
