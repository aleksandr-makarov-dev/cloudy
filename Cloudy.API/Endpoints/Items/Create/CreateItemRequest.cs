namespace Cloudy.API.Endpoints.Items.Create;

public record CreateItemRequest(string Name, Guid? ParentId);