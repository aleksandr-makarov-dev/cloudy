namespace Cloudy.API.Endpoints.Items.Get;

public record ItemResponse(Guid Id, string DisplayName, string? ContentType, long? Size, DateTime CreatedAt);