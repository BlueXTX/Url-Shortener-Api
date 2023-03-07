using FluentValidation;
using ShortUrl.Api.Validators;
using ShortUrl.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddShortUrl(builder.Configuration);
builder.Services.AddValidatorsFromAssemblyContaining<CreateShortLinkDtoValidator>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.Run();

public partial class Program {}
