using FluentValidation;

namespace Cloudy.API.Endpoints.Items.Upload;

public sealed class UploadItemRequestValidator : AbstractValidator<UploadItemRequest>
{
    public UploadItemRequestValidator()
    {
        RuleFor(x => x.File)
            .NotNull();
    }
}