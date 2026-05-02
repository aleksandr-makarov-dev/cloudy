namespace Cloudy.API.Domain;

public sealed class Item
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Extension { get; set; }

    public long? Size { get; set; }

    public string? ContentType { get; set; }

    public bool IsFolder { get; set; }

    public DateTime CreatedAt { get; set; }

    public Guid? ParentId { get; set; }

    public Item? Parent { get; set; }
}