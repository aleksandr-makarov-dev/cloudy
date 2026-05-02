using Cloudy.API.Endpoints.Items.Requests;
using FluentValidation;

namespace Cloudy.API.Endpoints.Items.Validators;

public sealed class UploadItemRequestValidator : AbstractValidator<UploadItemRequest>
{
    public UploadItemRequestValidator()
    {
        RuleFor(x => x.File)
            .NotNull();
    }
}