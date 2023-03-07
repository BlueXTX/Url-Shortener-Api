using FluentValidation;
using ShortUrl.Api.Dto;

namespace ShortUrl.Api.Validators;

public class CreateShortLinkDtoValidator : AbstractValidator<CreateShortLinkDto> {
    public CreateShortLinkDtoValidator()
    {
        RuleFor(x => x.OriginalUrl)
            .NotEmpty();
        RuleFor(x => x.OriginalUrl)
            .Must(value => Uri.TryCreate(value, UriKind.Absolute, out var url) &&
                           (url.Scheme == Uri.UriSchemeHttp || url.Scheme == Uri.UriSchemeHttps))
            .WithMessage(x => $"{x.OriginalUrl} is invalid url");
    }
}
