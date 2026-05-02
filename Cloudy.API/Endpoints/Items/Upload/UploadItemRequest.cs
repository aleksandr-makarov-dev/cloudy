namespace Cloudy.API.Endpoints.Items.Upload;

public record UploadItemRequest(IFormFile File, Guid? ParentId);