using FluentValidation;

namespace Cloudy.API.Features.Items.Upload;

public sealed class UploadItemRequestValidator : AbstractValidator<UploadItemRequest>
{
    public UploadItemRequestValidator()
    {
        RuleFor(x => x.File)
            .NotNull();
    }
}