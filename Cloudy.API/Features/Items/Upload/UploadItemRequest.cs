namespace Cloudy.API.Features.Items.Upload;

public record UploadItemRequest(IFormFile File, Guid? ParentId);