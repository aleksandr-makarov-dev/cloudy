namespace Cloudy.API.Features.Items.Create;

public record CreateItemRequest(string Name, Guid? ParentId);